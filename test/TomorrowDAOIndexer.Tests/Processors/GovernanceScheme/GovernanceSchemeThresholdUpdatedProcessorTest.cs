using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceSchemeThresholdUpdatedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(GovernanceSchemeThresholdUpdated(), GovernanceSchemeThresholdUpdatedProcessor);
         
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(GovernanceSchemeThresholdUpdated(), GovernanceSchemeThresholdUpdatedProcessor);
        var id = IdGenerateHelper.GetId(ChainId, DAOId, SchemeAddress);
        var governanceSchemeIndex = await GetIndexById<GovernanceSchemeIndex>(id);
        governanceSchemeIndex.ShouldNotBeNull();
        governanceSchemeIndex.MinimalRequiredThreshold.ShouldBe(1);
        governanceSchemeIndex.MinimalVoteThreshold.ShouldBe(1);
        governanceSchemeIndex.MinimalApproveThreshold.ShouldBe(1);
        governanceSchemeIndex.MaximalRejectionThreshold.ShouldBe(1);
        governanceSchemeIndex.MaximalAbstentionThreshold.ShouldBe(1);
    }
}