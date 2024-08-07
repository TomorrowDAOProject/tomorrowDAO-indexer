using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using ProposalStage = TomorrowDAO.Indexer.Plugin.Enums.ProposalStage;
using ProposalStatus = TomorrowDAOIndexer.Enums.ProposalStatus;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class ProposalExecutedProcessor : GovernanceProcessorBase<ProposalExecuted>
{
    public override async Task ProcessAsync(ProposalExecuted logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var proposalId = logEvent.ProposalId.ToHex();
        Logger.LogInformation("[ProposalExecuted] start proposalId:{proposalId} chainId:{chainId} ", proposalId,
            chainId);
        try
        {
            await UpdateProposal(proposalId, ProposalStatus.Executed, ProposalStage.Finished, logEvent.ExecuteTime?.ToDateTime(), string.Empty, string.Empty, context);
            Logger.LogInformation("[ProposalExecuted] end proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ProposalExecuted] Exception proposalId:{proposalId} chainId:{chainId}", proposalId, chainId);
            throw;
        }
    }
}