using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public abstract class GovernanceProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.GovernanceContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.GovernanceContractAddressMainNetSideChain,
            _ => throw new Exception("Unknown chain id")
        };
    }
    
    protected void UpdateProposal(string proposalId, ProposalStatus proposalStatus,
        ProposalStage proposalStage, string vetoProposalId, string beVetoedProposalId, LogEventContext context)
    {
        UpdateProposal(proposalId, proposalStatus, proposalStage, null, vetoProposalId, beVetoedProposalId, context);
    }

    protected async void UpdateProposal(string proposalId, ProposalStatus proposalStatus, 
        ProposalStage proposalStage, DateTime? executeTime, string vetoProposalId, string beVetoedProposalId, 
        LogEventContext context)
    {
        var proposalIndex = await GetEntityAsync<ProposalIndex>(proposalId);
        if (proposalIndex == null)
        {
            return;
        }
        proposalIndex.ProposalStatus = proposalStatus;
        proposalIndex.ProposalStage = proposalStage;
        proposalIndex.ExecuteTime = executeTime;
        if (!string.IsNullOrEmpty(vetoProposalId))
        {
            proposalIndex.VetoProposalId = vetoProposalId;
        }
        if (!string.IsNullOrEmpty(beVetoedProposalId))
        {
            proposalIndex.BeVetoedProposalId = beVetoedProposalId;
        }
        await SaveEntityAsync(proposalIndex, context);
    }
}