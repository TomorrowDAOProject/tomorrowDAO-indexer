using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using Newtonsoft.Json;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.Token;

public class TransferredProcessor : TokenProcessorBase<Transferred>
{
    public override async Task ProcessAsync(Transferred logEvent, LogEventContext context)
    {
        var treasuryAddress = logEvent.To.ToBase58();
        var chainId = context.ChainId;
        try
        {
            var treasuryCreateIndexId = IdGenerateHelper.GetId(chainId, treasuryAddress);
            var treasuryCreateIndex = await GetEntityAsync<TreasuryCreateIndex>(treasuryCreateIndexId);
            if (treasuryCreateIndex == null)
            {
                return;
            }
            Logger.LogInformation("[Transferred] START: to={treasuryAddress}, ChainId={ChainId}, Event={Event}",
                treasuryAddress, chainId, JsonConvert.SerializeObject(logEvent));
            var daoId = treasuryCreateIndex.DaoId;
            var symbol = logEvent.Symbol;

            await CreateOrUpdateTreasuryFundIndex(chainId, daoId, symbol, logEvent, context);
            await TreasuryStatistic(chainId, symbol, logEvent.Amount, context);

            var executor = logEvent.From?.ToBase58() ?? string.Empty;
            await SaveEntityAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId, executor,
                    TreasuryRecordType.Deposit),
                DaoId = daoId,
                TreasuryAddress = treasuryAddress,
                Executor = executor,
                FromAddress = logEvent.From?.ToBase58() ?? string.Empty,
                ToAddress = treasuryAddress,
                Amount = logEvent.Amount,
                Symbol = symbol,
                TreasuryRecordType = TreasuryRecordType.Deposit,
                CreateTime = context.Block.BlockTime
            }, context);

            Logger.LogInformation(
                "[Transferred] FINISH: daoId={Id}, ChainId={ChainId}, treasuryAddress={treasuryAddress}", daoId,
                chainId, treasuryAddress);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Transferred] Exception to={treasuryAddress}, ChainId={ChainId}", treasuryAddress,
                chainId);
            throw;
        }
    }

    private async Task CreateOrUpdateTreasuryFundIndex(string chainId, string daoId, string symbol,
        Transferred eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(chainId, daoId, symbol);
        var treasuryFundIndex = await GetEntityAsync<TreasuryFundIndex>(id) ?? new TreasuryFundIndex
        {
            Id = id,
            DaoId = daoId,
            TreasuryAddress = eventValue.To?.ToBase58() ?? string.Empty,
            Symbol = symbol
        };

        treasuryFundIndex.AvailableFunds += eventValue.Amount;
        await SaveEntityAsync(treasuryFundIndex, context);
    }
}