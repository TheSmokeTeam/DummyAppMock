using DummyAppMock.Processors;
using QaaS.Common.Generators.ConfigurationObjects.FromExternalSourceConfigurations;
using QaaS.Common.Generators.FromExternalSourceGenerators;
using QaaS.Framework.SDK.DataSourceObjects;
using QaaS.Mocker.Servers.ConfigurationObjects;
using QaaS.Mocker.Servers.ConfigurationObjects.HttpServerConfigs;
using QaaS.Mocker.Stubs.ConfigurationObjects;
using QaaS.Mocker;

if (ShouldUseCodeConfiguration(args, out var executionMode))
{
    if (executionMode == CodeExecutionMode.Template)
    {
        RenderCodeTemplate(args);
        return;
    }

    new MockerRunner(CreateCodeExecutionBuilder()).Run();
    return;
}

Bootstrap.New(NormalizeYamlArguments(args)).Run();

return;

static bool ShouldUseCodeConfiguration(string[] args, out CodeExecutionMode executionMode)
{
    if (args.Any(IsHelpOrVersionOption))
    {
        executionMode = default;
        return false;
    }

    if (args.Length == 0)
    {
        if (HasDefaultYamlConfiguration())
        {
            executionMode = default;
            return false;
        }

        executionMode = CodeExecutionMode.Run;
        return true;
    }

    if (args[0].Equals("template", StringComparison.OrdinalIgnoreCase) && !HasExplicitTemplateConfigurationPath(args))
    {
        executionMode = CodeExecutionMode.Template;
        return true;
    }

    executionMode = default;
    return false;
}

static ExecutionBuilder CreateCodeExecutionBuilder()
{
    return new ExecutionBuilder()
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
}

static void RenderCodeTemplate(IReadOnlyList<string> args)
{
    var template = """
DataSources:
  - Name: ServerData
    Generator: FromFileSystem
    GeneratorConfiguration:
      DataArrangeOrder: AsciiAsc
      FileSystem:
        Path: ServerData

Stubs:
  - Name: ServerDataStub
    Processor: ServerDataProcessor
    DataSourceNames: [ServerData]

Servers:
  - Http:
      Port: 8080
      IsLocalhost: false
      Endpoints:
        - Path: /data
          Actions:
            - Name: GetServerData
              Method: Get
              TransactionStubName: ServerDataStub
""";

    var outputFolder = GetTemplateOutputFolder(args);
    if (string.IsNullOrWhiteSpace(outputFolder))
    {
        Console.WriteLine(template);
        return;
    }

    Directory.CreateDirectory(outputFolder);
    File.WriteAllText(Path.Combine(outputFolder, "template.qaas.yaml"), template + Environment.NewLine);
}

static bool HasDefaultYamlConfiguration()
{
    return File.Exists(Path.Combine(AppContext.BaseDirectory, "mocker.qaas.yaml"));
}

static string[] NormalizeYamlArguments(string[] args)
{
    if (args.Length == 1 && LooksLikeConfigurationPath(args[0]))
        return ["run", args[0]];

    return args;
}

static bool HasExplicitTemplateConfigurationPath(IReadOnlyList<string> args)
{
    return args.Count > 1 && !args[1].StartsWith("-", StringComparison.Ordinal);
}

static string? GetTemplateOutputFolder(IReadOnlyList<string> args)
{
    for (var index = 1; index < args.Count; index++)
    {
        var argument = args[index];
        if ((argument.Equals("-o", StringComparison.OrdinalIgnoreCase) ||
             argument.Equals("--output-folder", StringComparison.OrdinalIgnoreCase)) &&
            index + 1 < args.Count)
        {
            return args[index + 1];
        }

        const string shortPrefix = "-o=";
        const string longPrefix = "--output-folder=";

        if (argument.StartsWith(shortPrefix, StringComparison.OrdinalIgnoreCase))
            return argument[shortPrefix.Length..];
        if (argument.StartsWith(longPrefix, StringComparison.OrdinalIgnoreCase))
            return argument[longPrefix.Length..];
    }

    return null;
}

static bool IsHelpOrVersionOption(string argument)
{
    return argument.Equals("--help", StringComparison.OrdinalIgnoreCase) ||
           argument.Equals("-h", StringComparison.OrdinalIgnoreCase) ||
           argument.Equals("--version", StringComparison.OrdinalIgnoreCase);
}

static bool LooksLikeConfigurationPath(string argument)
{
    if (argument.StartsWith("-", StringComparison.Ordinal))
        return false;

    if (Path.IsPathRooted(argument))
        return true;

    if (argument.IndexOfAny(['\\', '/']) >= 0)
        return true;

    var extension = Path.GetExtension(argument);
    return extension.Equals(".yaml", StringComparison.OrdinalIgnoreCase) ||
           extension.Equals(".yml", StringComparison.OrdinalIgnoreCase);
}

enum CodeExecutionMode
{
    Run,
    Template
}
