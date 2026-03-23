using DummyAppMock.Processors;
using QaaS.Common.Generators.ConfigurationObjects.FromExternalSourceConfigurations;
using QaaS.Common.Generators.FromExternalSourceGenerators;
using QaaS.Framework.SDK.DataSourceObjects;
using QaaS.Mocker;
using QaaS.Mocker.Servers.ConfigurationObjects;
using QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs;
using QaaS.Mocker.Stubs.ConfigurationObjects;

var executionBuilder = new ExecutionBuilder()
    .CreateDataSource(new DataSourceBuilder()
        .Named("ServerData")
        .HookNamed(nameof(FromFileSystem))
        .Configure(new FromFileSystemConfig
        {
            DataArrangeOrder = DataArrangeOrder.AsciiAsc,
            FileSystem = new FileSystemConfig
            {
                Path = Path.Combine(AppContext.BaseDirectory, "ServerData")
            }
        }))
    .CreateStub(new TransactionStubBuilder()
        .Named("ServerDataStub")
        .HookNamed(nameof(ServerDataProcessor))
        .AddDataSourceName("ServerData"))
    .ReplaceServers(
        new ServerConfig
        {
            Http = new HttpServerConfig
            {
                Port = 8080,
                IsLocalhost = false,
                Endpoints =
                [
                    new HttpEndpointConfig
                    {
                        Path = "/data",
                        Actions =
                        [
                            new HttpEndpointActionConfig
                            {
                                Name = "GetServerData",
                                Method = QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs.HttpMethod.Get,
                                TransactionStubName = "ServerDataStub"
                            }
                        ]
                    }
                ]
            }
        });

new MockerRunner(executionBuilder).Run();
