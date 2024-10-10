using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.Treasury;

public class TreasuryTransferredProcessor : TreasuryProcessorBase<TreasuryTransferred>
{
    public override async Task ProcessAsync(TreasuryTransferred logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var treasuryAddress = logEvent.TreasuryAddress?.ToBase58() ?? string.Empty;
        Logger.LogInformation("[TreasuryTransferReleased] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var symbol = logEvent.Symbol;
            var executor = logEvent.Executor.ToBase58();
            var daoIndex = await GetEntityAsync<DAOIndex>(daoId);
            var treasuryRecordIndex = ObjectMapper.Map<TreasuryTransferred, TreasuryRecordIndex>(logEvent);
            treasuryRecordIndex.Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId, executor,
                TreasuryRecordType.Transfer);
            treasuryRecordIndex.FromAddress = daoIndex?.TreasuryAccountAddress ?? string.Empty;
            treasuryRecordIndex.TreasuryRecordType = TreasuryRecordType.Transfer;
            await SaveEntityAsync(treasuryRecordIndex, context);
            await TreasuryFundSumStatistic(chainId, symbol, -logEvent.Amount, context);
            await TreasuryFundStatistic(chainId, daoId, symbol, treasuryAddress, -logEvent.Amount, context);
            Logger.LogInformation("[TreasuryTransferReleased] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryTransferReleased] Exception Id={DAOId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}