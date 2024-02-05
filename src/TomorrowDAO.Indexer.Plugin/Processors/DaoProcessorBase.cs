using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public abstract class DaoProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>,new()
{
    protected readonly ILogger<DaoProcessorBase<TEvent>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> DaoRepository;
    protected readonly IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> ElectionRepository;
    
    protected DaoProcessorBase(ILogger<DaoProcessorBase<TEvent>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        DaoRepository = daoRepository;
        ElectionRepository = electionRepository;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].DaoContractAddress;
    }

    protected async Task SaveIndexAsync(DaoIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await DaoRepository.AddOrUpdateAsync(index);
    }
    
    protected async Task SaveIndexAsync(ElectionIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await ElectionRepository.AddOrUpdateAsync(index);
    }
}