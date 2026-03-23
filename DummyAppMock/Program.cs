using DummyAppMock.Processors;
using System.Reflection;
using QaaS.Common.Generators.ConfigurationObjects.FromExternalSourceConfigurations;
using QaaS.Common.Generators.FromExternalSourceGenerators;
using QaaS.Framework.SDK.DataSourceObjects;
using QaaS.Mocker;
using QaaS.Mocker.Servers.ConfigurationObjects;
using QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs;
using QaaS.Mocker.Stubs.ConfigurationObjects;

var runner = Bootstrap.New(args);
var executionBuilder = GetExecutionBuilder(runner);

executionBuilder.CreateDataSource(new DataSourceBuilder()
    .Named("ServerData")
    .HookNamed(nameof(FromFileSystem))
    .Configure(new FromFileSystemConfig
    {
        DataArrangeOrder = DataArrangeOrder.AsciiAsc,
        FileSystem = new FileSystemConfig
        {
            Path = Path.Combine(AppContext.BaseDirectory, "ServerData")
        }
    }));

executionBuilder.CreateStub(new TransactionStubBuilder()
    .Named("ServerDataStub")
    .HookNamed(nameof(ServerDataProcessor))
    .AddDataSourceName("ServerData"));

executionBuilder.ReplaceServers(
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

runner.Run();

static ExecutionBuilder GetExecutionBuilder(MockerRunner runner)
{
    return typeof(MockerRunner)
        .GetField("_executionBuilder", BindingFlags.Instance | BindingFlags.NonPublic)?
        .GetValue(runner) as ExecutionBuilder
        ?? throw new InvalidOperationException("Mocker execution builder was not initialized.");
}
