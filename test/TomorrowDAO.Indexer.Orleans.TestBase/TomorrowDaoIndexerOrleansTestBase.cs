using Orleans.TestingHost;
using TomorrowDAO.Indexer.TestBase;
using Volo.Abp.Modularity;

namespace TomorrowDAO.Indexer.Orleans.TestBase;

public abstract class TomorrowDaoIndexerOrleansTestBase<TStartupModule> : TomorrowDAOIndexerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    protected readonly TestCluster Cluster;

    public TomorrowDaoIndexerOrleansTestBase()
    {
        Cluster = GetRequiredService<ClusterFixture>().Cluster;
    }
}