using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Treasury;

public abstract class TreasuryProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>,new()
{
    protected readonly ILogger<DAOProcessorBase<TEvent>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> TreasuryFundRepository;
    protected readonly IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> TreasuryRecordRepository;
    protected readonly IDAOProvider DAOProvider;
    private const int InitialAvailableFunds = 0;
    private const int InitialLockedFunds = 0;
    
    protected TreasuryProcessorBase(ILogger<DAOProcessorBase<TEvent>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository, 
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository, 
        IDAOProvider DAOProvider) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        TreasuryFundRepository = treasuryFundRepository;
        TreasuryRecordRepository = treasuryRecordRepository;
        this.DAOProvider = DAOProvider;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].DAOContractAddress;
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
    
    protected async Task AddSymbolList(List<string> symbolList, string DAOId, LogEventContext context)
    {
        var chainId = context.ChainId;
        foreach (var symbol in symbolList)
        {
            var id = IdGenerateHelper.GetId(chainId, DAOId, symbol);
            var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(id, chainId);
            if (treasuryFundIndex == null)
            {
                treasuryFundIndex = new TreasuryFundIndex
                {
                    Id = id,
                    DAOId = DAOId,
                    Symbol = symbol,
                    AvailableFunds = InitialAvailableFunds,
                    LockedFunds = InitialLockedFunds,
                };
            }
            ObjectMapper.Map(context, treasuryFundIndex);
            await TreasuryFundRepository.AddOrUpdateAsync(treasuryFundIndex);
        }
    }
}