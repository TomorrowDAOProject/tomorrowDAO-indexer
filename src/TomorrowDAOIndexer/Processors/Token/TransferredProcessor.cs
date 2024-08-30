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
        var treasuryAddress = logEvent.To?.ToBase58() ?? string.Empty;
        var executor = logEvent.From?.ToBase58() ?? string.Empty;
        var symbol = logEvent.Symbol;
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
            var treasuryRecordIndex = ObjectMapper.Map<Transferred, TreasuryRecordIndex>(logEvent);
            treasuryRecordIndex.Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId, executor,
                TreasuryRecordType.Deposit);
            treasuryRecordIndex.DaoId = daoId;
            treasuryRecordIndex.TreasuryAddress = treasuryAddress;
            treasuryRecordIndex.TreasuryRecordType = TreasuryRecordType.Deposit;
            await SaveEntityAsync(treasuryRecordIndex, context);
            await TreasuryFundStatistic(chainId, daoId, symbol, treasuryAddress, logEvent.Amount, context);
            await TreasuryFundSumStatistic(chainId, symbol, logEvent.Amount, context);

            Logger.LogInformation("[Transferred] FINISH: daoId={Id}, ChainId={ChainId}, treasuryAddress={treasuryAddress}", 
                daoId, chainId, treasuryAddress);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Transferred] Exception to={treasuryAddress}, ChainId={ChainId}", treasuryAddress,
                chainId);
            throw;
        }
    }
}