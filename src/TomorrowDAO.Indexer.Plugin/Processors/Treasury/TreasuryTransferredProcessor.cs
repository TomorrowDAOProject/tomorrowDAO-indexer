using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Treasury;

public class TreasuryTransferredProcessor : TreasuryProcessorBase<TreasuryTransferred>
{
    public TreasuryTransferredProcessor(
        ILogger<AElfLogEventProcessorBase<TreasuryTransferred, LogEventInfo>> logger, IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository,
        IDAOProvider DAOProvider, ITreasuryProvider TreasuryProvider)
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider, TreasuryProvider)
    {
    }

    protected override async Task HandleEventAsync(TreasuryTransferred eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[TreasuryTransferReleased] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var symbol = eventValue.Symbol;
            var id = IdGenerateHelper.GetId(chainId, DAOId, symbol);
            var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(id, chainId);
            if (treasuryFundIndex == null)
            {
                Logger.LogInformation("[TreasuryTransferReleased] TreasuryFund not support symbol: Id={Id}, ChainId={ChainId}, Symbol={Symbol}", 
                    id, chainId, symbol);
                return;
            }
            treasuryFundIndex.AvailableFunds -= eventValue.Amount;
            await SaveIndexAsync(treasuryFundIndex, context);
            var executor = eventValue.Executor.ToBase58();
            var daoIndex = await DAOProvider.GetDaoAsync(chainId, DAOId);
            await SaveIndexAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.TransactionId, executor, TreasuryRecordType.Transfer),
                DaoId = DAOId,
                TreasuryAddress = eventValue.TreasuryAddress?.ToBase58(),
                Executor = executor,
                FromAddress = daoIndex?.TreasuryAccountAddress,
                ToAddress = eventValue.Recipient?.ToBase58(),
                Amount = eventValue.Amount,
                Symbol = symbol,
                TreasuryRecordType = TreasuryRecordType.Transfer,
                CreateTime = context.BlockTime,
                Memo = eventValue.Memo
            }, context);
            await TreasuryProvider.TreasuryStatistic(chainId, symbol, -eventValue.Amount, context);
            Logger.LogInformation("[TreasuryTransferReleased] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryTransferReleased] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}