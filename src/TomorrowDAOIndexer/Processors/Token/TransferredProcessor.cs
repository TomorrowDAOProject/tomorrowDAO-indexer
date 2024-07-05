using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;
using Newtonsoft.Json;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Token;

public class TransferredProcessor : TokenProcessorBase<Transferred>
{
    public override async Task ProcessAsync(Transferred logEvent, LogEventContext context)
    {
        var treasuryAddress = logEvent.To.ToBase58();
        var chainId = context.ChainId;

        Logger.LogInformation("[Transferred] START: to={treasuryAddress}, ChainId={ChainId}, Event={Event}",
            treasuryAddress, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var treasuryCreateIndexId = IdGenerateHelper.GetId(chainId, treasuryAddress);
            var treasuryCreateIndex = await GetEntityAsync<TreasuryCreateIndex>(treasuryCreateIndexId);
            if (treasuryCreateIndex == null)
            {
                Logger.LogInformation(
                    "[Transferred] to address is not treasury address, to ={treasuryAddress}, ChainId={ChainId}",
                    treasuryAddress, chainId);
                return;
            }

            var daoId = treasuryCreateIndex.DaoId;
            var symbol = logEvent.Symbol;

            await CreateOrUpdateTreasuryFundIndex(chainId, daoId, symbol, logEvent, context);
            await TreasuryStatistic(chainId, symbol, logEvent.Amount, context);

            var executor = logEvent.From?.ToBase58();
            await SaveEntityAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId, executor,
                    TreasuryRecordType.Deposit),
                DaoId = daoId,
                TreasuryAddress = treasuryAddress,
                Executor = executor,
                FromAddress = logEvent.From?.ToBase58(),
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
        var treasuryFundIndex = await GetEntityAsync<TreasuryFundIndex>(id);
        if (treasuryFundIndex == null)
        {
            Logger.LogInformation(
                "[Transferred] TreasuryFund not support symbol: Id={Id}, ChainId={ChainId}, Symbol={Symbol}",
                daoId, chainId, symbol);
            treasuryFundIndex = new TreasuryFundIndex
            {
                Id = id,
                DaoId = daoId,
                TreasuryAddress = eventValue.To?.ToBase58(),
                Symbol = symbol
            };
        }

        treasuryFundIndex.AvailableFunds += eventValue.Amount;
        await SaveEntityAsync(treasuryFundIndex, context);
        Logger.LogInformation(
            "[Transferred] TreasuryFund FINISH: Id={Id}, ChainId={ChainId}, Symbol={Symbol}", daoId, chainId, symbol);
    }
}