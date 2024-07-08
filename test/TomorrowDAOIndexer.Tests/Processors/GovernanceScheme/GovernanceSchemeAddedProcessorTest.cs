using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceSchemeAddedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var id = IdGenerateHelper.GetId(ChainId, DAOId, SchemeAddress);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        var governanceSchemeIndex = await GetIndexById<GovernanceSchemeIndex>(id);
        governanceSchemeIndex.ShouldNotBeNull();
        governanceSchemeIndex.Id.ShouldBe(id);
        governanceSchemeIndex.DAOId.ShouldBe(DAOId);
        governanceSchemeIndex.SchemeId.ShouldBe(SchemeId);
        governanceSchemeIndex.SchemeAddress.ShouldBe(SchemeAddress);
        governanceSchemeIndex.GovernanceToken.ShouldBe(Elf);
        governanceSchemeIndex.GovernanceMechanism.ShouldBe(GovernanceMechanism.Referendum);
        governanceSchemeIndex.MinimalRequiredThreshold.ShouldBe(10);
        governanceSchemeIndex.MinimalVoteThreshold.ShouldBe(12);
        governanceSchemeIndex.MinimalApproveThreshold.ShouldBe(50);
        governanceSchemeIndex.MaximalRejectionThreshold.ShouldBe(30);
        governanceSchemeIndex.MaximalAbstentionThreshold.ShouldBe(20);
        governanceSchemeIndex.ProposalThreshold.ShouldBe(10);
         
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
    }
}