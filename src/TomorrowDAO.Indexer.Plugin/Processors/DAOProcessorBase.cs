using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public abstract class DAOProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>,new()
{
    protected readonly ILogger<DAOProcessorBase<TEvent>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository;
    protected readonly IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> ElectionRepository;
    
    protected DAOProcessorBase(ILogger<DAOProcessorBase<TEvent>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        this.DAORepository = DAORepository;
        ElectionRepository = electionRepository;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].DAOContractAddress;
    }

    protected async Task SaveIndexAsync(DAOIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await DAORepository.AddOrUpdateAsync(index);
    }
    
    protected async Task SaveIndexAsync(ElectionIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await ElectionRepository.AddOrUpdateAsync(index);
    }
}