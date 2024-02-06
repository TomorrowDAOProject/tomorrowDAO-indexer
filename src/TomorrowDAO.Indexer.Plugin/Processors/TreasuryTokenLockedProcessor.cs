using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class TreasuryTokenLockedProcessor : TreasuryProcessorBase<TreasuryTokenLocked>
{
    public TreasuryTokenLockedProcessor(ILogger<DAOProcessorBase<TreasuryTokenLocked>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository, IDAOProvider DAOProvider) 
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(TreasuryTokenLocked eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[TreasuryTokenLocked] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var lockInfo = eventValue.LockInfo;
            var symbol = lockInfo.Symbol;
            var id = IdGenerateHelper.GetId(chainId, DAOId, symbol);
            var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(id, chainId);
            if (treasuryFundIndex == null)
            {
                Logger.LogInformation("[TreasuryTokenLocked] TreasuryFund not support symbol: Id={Id}, ChainId={ChainId}, Symbol={Symbol}", 
                    id, chainId, symbol);
                return;
            }
            treasuryFundIndex.AvailableFunds -= lockInfo.Amount;
            treasuryFundIndex.LockedFunds += lockInfo.Amount;
            await SaveIndexAsync(treasuryFundIndex, context);
            var executor = eventValue.Proposer.ToBase58();
            await SaveIndexAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.TransactionId, executor, TreasuryRecordType.Donate),
                DAOId = DAOId,
                Executor = executor,
                From = string.Empty,
                To = string.Empty,
                Amount = lockInfo.Amount,
                Symbol = symbol,
                TreasuryRecordType = TreasuryRecordType.Lock,
                CreateTime = context.BlockTime
            }, context);
            Logger.LogInformation("[TreasuryTokenLocked] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryTokenLocked] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}