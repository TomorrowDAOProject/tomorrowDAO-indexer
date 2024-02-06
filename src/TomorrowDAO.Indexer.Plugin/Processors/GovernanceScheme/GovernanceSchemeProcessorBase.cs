using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

public abstract class GovernanceSchemeProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;

    protected readonly IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo>
        GovernanceSchemeRepository;
    protected readonly IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo>
        GovernanceSubSchemeRepository;

    protected GovernanceSchemeProcessorBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository, 
        IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository) :
        base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        ContractInfoOptions = contractInfoOptions.Value;
        GovernanceSchemeRepository = governanceSchemeRepository;
        GovernanceSubSchemeRepository = governanceSubSchemeRepository;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].GovernanceContract;
    }

    protected async Task SaveIndexAsync(GovernanceSchemeIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await GovernanceSchemeRepository.AddOrUpdateAsync(index);
    }
    
    protected async Task SaveIndexAsync(GovernanceSubSchemeIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await GovernanceSubSchemeRepository.AddOrUpdateAsync(index);
    }
}