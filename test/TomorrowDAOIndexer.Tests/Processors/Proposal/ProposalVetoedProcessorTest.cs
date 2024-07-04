using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class ProposalVetoedProcessorTest :TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task ProposalVetoed_Test()
    {
        //ProposalId
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        //Id4
        await MockEventProcess(ProposalCreated_Veto(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalExecuted(Id4), ProposalExecutedProcessor);
        await MockEventProcess(ProposalVetoed(), ProposalVetoedProcessor);
        
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex = await GetIndexById<ProposalIndex>(proposalId);
        proposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Vetoed);
        proposalIndex.ProposalStage.ShouldBe(ProposalStage.Finished);
        
        var vetoProposalIndex = await GetIndexById<ProposalIndex>(VetoProposalId);
        vetoProposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Executed);
        vetoProposalIndex.ProposalStage.ShouldBe(ProposalStage.Finished);
    }
}