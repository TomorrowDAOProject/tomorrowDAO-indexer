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
        Logger.LogInformation("[TreasuryTransferReleased] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var symbol = logEvent.Symbol;
            var id = IdGenerateHelper.GetId(chainId, daoId, symbol);
            var treasuryFundIndex = await GetEntityAsync<TreasuryFundIndex>(id);
            if (treasuryFundIndex == null)
            {
                Logger.LogInformation(
                    "[TreasuryTransferReleased] TreasuryFund not exist, Id={Id}, ChainId={ChainId}, Symbol={Symbol}",
                    id, chainId, symbol);
                return;
            }
            else
            {
                treasuryFundIndex.AvailableFunds -= logEvent.Amount;
                await SaveEntityAsync(treasuryFundIndex, context);
                Logger.LogInformation(
                    "[TreasuryTransferReleased] TreasuryFund Updated, Id={Id}, AvailableFunds={AvailableFunds}, Symbol={Symbol}",
                    id, treasuryFundIndex.AvailableFunds.ToString(), symbol);
            }

            var executor = logEvent.Executor.ToBase58();
            var daoIndex = await GetEntityAsync<DAOIndex>(daoId);
            await SaveEntityAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId, executor, TreasuryRecordType.Transfer),
                DaoId = daoId,
                TreasuryAddress = logEvent.TreasuryAddress?.ToBase58(),
                Executor = executor,
                FromAddress = daoIndex?.TreasuryAccountAddress,
                ToAddress = logEvent.Recipient?.ToBase58(),
                Amount = logEvent.Amount,
                Symbol = symbol,
                TreasuryRecordType = TreasuryRecordType.Transfer,
                CreateTime = context.Block.BlockTime,
                Memo = logEvent.Memo
            }, context);
            await TreasuryStatistic(chainId, symbol, -logEvent.Amount, context);
            Logger.LogInformation("[TreasuryTransferReleased] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryTransferReleased] Exception Id={DAOId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}