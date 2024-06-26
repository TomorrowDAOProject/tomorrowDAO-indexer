using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Election;

public abstract class ElectionProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected const int CandidateTerm = 0;
    protected readonly ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> ElectionRepository;
    protected readonly IElectionProvider ElectionProvider;

    protected ElectionProcessorBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        ElectionRepository = electionRepository;
        ElectionProvider = electionProvider;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].ElectionContractAddress;
    }

    protected async Task SaveIndexAsync(ElectionIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await ElectionRepository.AddOrUpdateAsync(index);
    }
}