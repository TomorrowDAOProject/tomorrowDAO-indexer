using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class ProposalExecutedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalExecuted(), ProposalExecutedProcessor);
        
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex = await GetIndexById<ProposalIndex>(proposalId);
        proposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Executed);
        proposalIndex.ProposalStage.ShouldBe(ProposalStage.Finished);
    }
}