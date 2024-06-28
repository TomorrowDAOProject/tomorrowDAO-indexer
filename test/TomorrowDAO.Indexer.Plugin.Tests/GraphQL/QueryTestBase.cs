using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

[CollectionDefinition(ClusterCollection.Name)]
public class QueryTestBase : TomorrowDAOIndexerPluginTestBase
{
    protected readonly IObjectMapper ObjectMapper;

    protected QueryTestBase()
    {
        ObjectMapper = GetRequiredService<IObjectMapper>();
    }
}