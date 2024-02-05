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
    protected readonly ILogger<DaoProcessorBase<TEvent>> _logger;
    protected readonly IObjectMapper _objectMapper;
    protected readonly ContractInfoOptions _contractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> _daoRepository;
    protected readonly IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> _electionRepository;
    
    protected DaoProcessorBase(ILogger<DaoProcessorBase<TEvent>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger)
    {
        _logger = logger;
        _objectMapper = objectMapper;
        _daoRepository = daoRepository;
        _electionRepository = electionRepository;
        _contractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return _contractInfoOptions.ContractInfos[chainId].DaoContractAddress;
    }

    protected async Task SaveIndexAsync(DaoIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _daoRepository.AddOrUpdateAsync(index);
    }
    
    protected async Task SaveIndexAsync(ElectionIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _electionRepository.AddOrUpdateAsync(index);
    }
}