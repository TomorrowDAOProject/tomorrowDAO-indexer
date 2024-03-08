using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using ProposalStage = TomorrowDAO.Indexer.Plugin.Enums.ProposalStage;
using ProposalStatus = TomorrowDAO.Indexer.Plugin.Enums.ProposalStatus;

namespace TomorrowDAO.Indexer.Plugin.Processors.Proposal;

public class ProposalExecutedProcessor : ProposalProcessorBase<ProposalExecuted>
{
    public ProposalExecutedProcessor(ILogger<AElfLogEventProcessorBase<ProposalExecuted, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> proposalRepository,
        IGovernanceProvider governanceProvider, IDAOProvider DAOProvider) :
        base(logger, objectMapper, contractInfoOptions, proposalRepository, governanceProvider, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(ProposalExecuted eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId.ToHex();
        Logger.LogInformation("[ProposalExecuted] start proposalId:{proposalId} chainId:{chainId} ", proposalId,
            chainId);
        try
        {
            UpdateVetoProposal(proposalId, ProposalStatus.Executed, ProposalStage.Finished, eventValue.ExecuteTime?.ToDateTime(), context);
            Logger.LogInformation("[ProposalExecuted] end proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}