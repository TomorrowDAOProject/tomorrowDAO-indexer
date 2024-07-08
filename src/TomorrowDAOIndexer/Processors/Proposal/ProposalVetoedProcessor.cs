using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using ProposalStage = TomorrowDAO.Indexer.Plugin.Enums.ProposalStage;
using ProposalStatus = TomorrowDAO.Indexer.Plugin.Enums.ProposalStatus;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class ProposalVetoedProcessor : GovernanceProcessorBase<ProposalVetoed>
{
    public override async Task ProcessAsync(ProposalVetoed logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var proposalId = logEvent.ProposalId?.ToHex();
        var vetoProposalId = logEvent.VetoProposalId?.ToHex();
        Logger.LogInformation("[ProposalVetoed] start proposalId:{proposalId} chainId:{chainId} vetoProposalId {vetoProposalId}", proposalId, chainId, vetoProposalId);
        try
        {
            await UpdateProposal(vetoProposalId, ProposalStatus.Vetoed, ProposalStage.Finished, string.Empty, string.Empty, context);
            Logger.LogInformation("[ProposalVetoed] end proposalId:{proposalId} chainId:{chainId} vetoProposalId {vetoProposalId}", proposalId, chainId, vetoProposalId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ProposalVetoed] Exception proposalId:{proposalId} chainId:{chainId} vetoProposalId {vetoProposalId}", proposalId, chainId, vetoProposalId);
            throw;
        }
    }
}