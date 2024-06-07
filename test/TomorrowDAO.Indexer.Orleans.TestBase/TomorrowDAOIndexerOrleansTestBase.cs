using Orleans.TestingHost;
using TomorrowDAO.Indexer.TestBase;
using Volo.Abp.Modularity;

namespace TomorrowDAO.Indexer.Orleans.TestBase;

public abstract class TomorrowDAOIndexerOrleansTestBase<TStartupModule> : TomorrowDAOIndexerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    
}