using AElfIndexer.Client.GraphQL;

namespace TomorrowDAO.Indexer.Plugin.GraphQL;

public class TomorrowDAOIndexerPluginSchema : AElfIndexerClientSchema<Query>
{
    public TomorrowDAOIndexerPluginSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}