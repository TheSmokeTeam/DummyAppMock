using DummyAppMock.Processors;
using QaaS.Common.Generators.ConfigurationObjects.FromExternalSourceConfigurations;
using QaaS.Common.Generators.FromExternalSourceGenerators;
using QaaS.Framework.SDK.DataSourceObjects;
using QaaS.Mocker;
using QaaS.Mocker.Servers.ConfigurationObjects;
using QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs;
using QaaS.Mocker.Stubs.ConfigurationObjects;
using HttpMethod = QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs.HttpMethod;

namespace DummyAppMock;

public sealed class MockerExecutionBuilderConfigurator
{
    public void Configure(ExecutionBuilder executionBuilder)
    {
        executionBuilder
            .CreateDataSource(new DataSourceBuilder()
                .Named("ServerData")
                .HookNamed(nameof(FromFileSystem))
                .Configure(new FromFileSystemConfig
                {
                    DataArrangeOrder = DataArrangeOrder.AsciiAsc,
                    FileSystem = new FileSystemConfig
                    {
                        Path = "ServerData"
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
                        IsLocalhost = true,
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
                                        Method = HttpMethod.Get,
                                        TransactionStubName = "ServerDataStub"
                                    }
                                ]
                            }
                        ]
                    }
                });
    }
}
