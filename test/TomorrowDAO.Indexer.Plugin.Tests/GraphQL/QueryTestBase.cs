using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class QueryTestBase : TomorrowDAOIndexerPluginTestBase
{
    protected readonly IObjectMapper ObjectMapper;

    protected QueryTestBase()
    {
        ObjectMapper = GetRequiredService<IObjectMapper>();
    }
}