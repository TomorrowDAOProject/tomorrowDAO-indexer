using AeFinder.Sdk;

namespace TomorrowDAOIndexer.GraphQL;

public class AppSchema : AppSchema<Query>
{
    public AppSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}