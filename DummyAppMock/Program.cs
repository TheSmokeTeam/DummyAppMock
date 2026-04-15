using DummyAppMock.Processors;
using QaaS.Common.Generators.ConfigurationObjects.FromExternalSourceConfigurations;
using QaaS.Common.Generators.FromExternalSourceGenerators;
using QaaS.Framework.SDK.DataSourceObjects;
using QaaS.Framework.SDK.Extensions;
using QaaS.Mocker;
using QaaS.Mocker.Servers.ConfigurationObjects;
using QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs;
using QaaS.Mocker.Stubs.ConfigurationObjects;
using HttpMethod = QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs.HttpMethod;

var bootstrapArguments = args.Length > 0 ? args : ["run", "mocker.qaas.yaml"];
var runner = Bootstrap.New(bootstrapArguments);
var executionBuilder = runner.ExecutionBuilders.AsSingle();

var dataSource = new DataSourceBuilder()
    .Named("ServerData")
    .HookNamed(nameof(FromFileSystem))
    .Configure(new FromFileSystemConfig
    {
        DataArrangeOrder = DataArrangeOrder.AsciiAsc,
        FileSystem = new FileSystemConfig
        {
            Path = "ServerData"
        }
    });

var stub = new TransactionStubBuilder()
    .Named("ServerDataStub")
    .HookNamed(nameof(ServerDataProcessor))
    .AddDataSourceName("ServerData");

var server = new ServerConfig
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
                        Name = "GetServerData", Method = HttpMethod.Get, TransactionStubName = "ServerDataStub"
                    }
                ]
            }
        ]
    }
};

executionBuilder
    .AddDataSource(dataSource)
    .AddStub(stub)
    .AddServer(server);

runner.Run();