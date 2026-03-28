using System.Collections.Immutable;
using QaaS.Framework.SDK.DataSourceObjects;
using QaaS.Framework.SDK.Extensions;
using QaaS.Framework.SDK.Hooks.Processor;
using QaaS.Framework.SDK.Session.DataObjects;
using QaaS.Framework.SDK.Session.MetaDataObjects;

namespace DummyAppMock.Processors;

public sealed class ServerDataProcessor : BaseTransactionProcessor<NoConfiguration>
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
                    StatusCode = 200,
                    ResponseHeaders = new Dictionary<string, string>
                    {
                        ["Content-Type"] = "application/json"
                    }
                }
            }
        };
    }
}

public sealed record NoConfiguration;
