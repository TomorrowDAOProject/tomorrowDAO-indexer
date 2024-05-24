using Xunit;

namespace TomorrowDAO.Indexer.Orleans.TestBase;

[CollectionDefinition(Name)]
public class ClusterCollection:ICollectionFixture<ClusterFixture>
{
    public const string Name = "ClusterCollection";
}