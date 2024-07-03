using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using Xunit;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceSchemeThresholdRemovedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task GovernanceSchemeThresholdRemoved_Test()
    {
        await MockEventProcess(GovernanceSchemeThresholdRemoved(), GovernanceSchemeThresholdRemovedProcessor);
         
        var id = IdGenerateHelper.GetId(ChainId, DAOId, SchemeAddress);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(GovernanceSchemeThresholdRemoved(), GovernanceSchemeThresholdRemovedProcessor);
        var governanceSchemeIndex = await GetIndexById(id, GovernanceSchemeRepository);
        governanceSchemeIndex.ShouldBeNull();
    }
}