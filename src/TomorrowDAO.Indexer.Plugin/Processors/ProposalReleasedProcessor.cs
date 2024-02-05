using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using ProposalStatus = TomorrowDAO.Indexer.Plugin.Enums.ProposalStatus;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class ProposalReleasedProcessor : ProposalProcessorBase<ProposalReleased>
{
    public ProposalReleasedProcessor(ILogger<AElfLogEventProcessorBase<ProposalReleased, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> proposalRepository,
        IGovernanceSchemeProvider governanceSchemeProvider) :
        base(logger, objectMapper, contractInfoOptions, proposalRepository, governanceSchemeProvider)
    {
    }

    protected override async Task HandleEventAsync(ProposalReleased eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId.ToHex();
        Logger.LogInformation("[ProposalReleased] start proposalId:{proposalId} chainId:{chainId} ", proposalId,
            chainId);
        var proposalIndex = await ProposalRepository.GetFromBlockStateSetAsync(proposalId, context.ChainId);
        if (proposalIndex == null)
        {
            Logger.LogInformation("[ProposalReleased] proposalIndex with id {id} chainId {chainId} has not existed.",
                proposalId, chainId);
            return;
        }

        ObjectMapper.Map(context, proposalIndex);
        proposalIndex.ProposalStatus = ProposalStatus.Executed;
        await ProposalRepository.AddOrUpdateAsync(proposalIndex);
        Logger.LogInformation("[ProposalReleased] end proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
    }
}