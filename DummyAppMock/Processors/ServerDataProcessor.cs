using System.Collections.Immutable;
using QaaS.Framework.SDK.DataSourceObjects;
using QaaS.Framework.SDK.Extensions;
using QaaS.Framework.SDK.Hooks.Processor;
using QaaS.Framework.SDK.Session.DataObjects;
using QaaS.Framework.SDK.Session.MetaDataObjects;
using System.ComponentModel;

namespace DummyAppMock.Processors;

public record ServerDataProcessorConfig
{
    [Description("Status code to return"), DefaultValue("200")]
    public int StatusCode { get; set; } = 200;
}

public class ServerDataProcessor : BaseTransactionProcessor<ServerDataProcessorConfig>
{
    public override Data<object> Process(IImmutableList<DataSource> dataSourceList, Data<object> requestData)
    {
        var response = dataSourceList
            .GetDataSourceByName("ServerData")
            .Retrieve()
            .FirstOrDefault();

        return new Data<object>
        {
            Body = response?.Body ?? "{}"u8.ToArray(),
            MetaData = new MetaData
            {
                Http = new Http
                {
                    StatusCode = Configuration.StatusCode,
                    ResponseHeaders = new Dictionary<string, string>
                    {
                        ["Content-Type"] = "application/json"
                    }
                }
            }
        };
    }
}