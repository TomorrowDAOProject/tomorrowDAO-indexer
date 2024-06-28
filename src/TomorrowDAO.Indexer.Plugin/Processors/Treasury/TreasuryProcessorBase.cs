using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Treasury;

public abstract class TreasuryProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>,new()
{
    protected readonly ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> TreasuryFundRepository;
    protected readonly IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> TreasuryRecordRepository;
    protected readonly IDAOProvider DAOProvider;
    protected readonly ITreasuryProvider TreasuryProvider;

    protected TreasuryProcessorBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository,
        IDAOProvider DAOProvider, ITreasuryProvider treasuryProvider) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        TreasuryFundRepository = treasuryFundRepository;
        TreasuryRecordRepository = treasuryRecordRepository;
        this.DAOProvider = DAOProvider;
        TreasuryProvider = treasuryProvider;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].TreasuryContractAddress;
    }

    protected async Task SaveIndexAsync(TreasuryFundIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await TreasuryFundRepository.AddOrUpdateAsync(index);
    }
    
    protected async Task SaveIndexAsync(TreasuryRecordIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await TreasuryRecordRepository.AddOrUpdateAsync(index);
    }
}