using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using Volo.Abp.ObjectMapping;
using ProposalCreated = AElf.Standards.ACS3.ProposalCreated;

namespace TomorrowDAO.Indexer.Plugin.Processors.NetworkDao.Parliament;

public class ParliamentProposalCreatedProcessor : ParliamentProcessorBase<ProposalCreated>
{
    public ParliamentProposalCreatedProcessor(ILogger<AElfLogEventProcessorBase<ProposalCreated, LogEventInfo>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<NetworkDaoProposalIndex, LogEventInfo> proposalRepository) : base(logger,
        objectMapper, contractInfoOptions, proposalRepository)
    {
    }

    protected override async Task HandleEventAsync(ProposalCreated eventValue, LogEventContext context)
    {
        Logger.LogInformation("[Parliament ProposalCreated] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, eventValue.ProposalId?.ToHex());
        await SaveProposalIndex(eventValue, context, NetworkDaoProposalType.Parliament);
    }
}