using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Election;

public class ElectionVotingEventRegisteredProcessor : ElectionProcessorBase<ElectionVotingEventRegistered>
{
    public ElectionVotingEventRegisteredProcessor(
        ILogger<AElfLogEventProcessorBase<ElectionVotingEventRegistered, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper,
        contractInfoOptions, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(ElectionVotingEventRegistered eventValue, LogEventContext context)
    {
    }
}