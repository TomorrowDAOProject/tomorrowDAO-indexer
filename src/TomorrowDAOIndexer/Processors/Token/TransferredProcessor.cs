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
        await ProcessTreasuryAsync(logEvent, context);
        await ProcessBalanceAsync(logEvent, context);
    }

    private async Task ProcessBalanceAsync(Transferred logEvent, LogEventContext context)
    {
        var toAddress = logEvent.To.ToBase58();
        var fromAddress = logEvent.From.ToBase58();
        var amount = logEvent.Amount;
        var symbol = logEvent.Symbol;
        var chainId = context.ChainId;
        try
        {
            if (!CheckSymbol(chainId, symbol))
            {
                return;
            }
            
            Logger.LogInformation("[Transferred] ProcessBalanceAsyncSTART: ChainId={ChainId}, from={fromAddress}, to={toAddress}, amount={amount}",
                chainId, fromAddress, toAddress, amount);
            await SaveUserBalanceAsync(symbol, toAddress, amount, context);
            await SaveUserBalanceAsync(symbol, fromAddress, -amount, context);
            Logger.LogInformation("[Transferred] ProcessBalanceAsyncFINISH: ChainId={ChainId}, from={fromAddress}, to={toAddress}, amount={amount}",
                chainId, fromAddress, toAddress, amount);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Transferred] ProcessBalanceAsyncFINISH: ChainId={ChainId}, from={fromAddress}, to={toAddress}, amount={amount}",
                chainId, fromAddress, toAddress, amount);
            throw;
        }
    }

    private async Task ProcessTreasuryAsync(Transferred logEvent, LogEventContext context)
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
            Logger.LogInformation("[Transferred] ProcessTreasuryAsyncSTART: to={treasuryAddress}, ChainId={ChainId}, Event={Event}",
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
                "[Transferred] ProcessTreasuryAsyncFINISH: daoId={Id}, ChainId={ChainId}, treasuryAddress={treasuryAddress}", daoId,
                chainId, treasuryAddress);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Transferred] ProcessTreasuryAsyncException to={treasuryAddress}, ChainId={ChainId}", treasuryAddress,
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