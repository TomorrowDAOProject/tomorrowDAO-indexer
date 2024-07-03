using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using Volo.Abp.ObjectMapping;
using ProposalStage = TomorrowDAO.Indexer.Plugin.Enums.ProposalStage;
using ProposalStatus = TomorrowDAO.Indexer.Plugin.Enums.ProposalStatus;

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
            UpdateProposal(proposalId, ProposalStatus.Executed, ProposalStage.Finished, logEvent.ExecuteTime?.ToDateTime(), string.Empty, string.Empty, context);
            Logger.LogInformation("[ProposalExecuted] end proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}