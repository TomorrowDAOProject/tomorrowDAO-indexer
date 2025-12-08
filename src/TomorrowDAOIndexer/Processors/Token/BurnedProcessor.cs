using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;

namespace TomorrowDAOIndexer.Processors.Token;

public class BurnedProcessor : TokenProcessorBase<Burned>
{
    public override async Task ProcessAsync(Burned logEvent, LogEventContext context)
    {
        var fromAddress = logEvent.Burner.ToBase58();
        var amount = logEvent.Amount;
        var symbol = logEvent.Symbol;
        var chainId = context.ChainId;
        try
        {
            if (!CheckSymbol(chainId, symbol))
            {
                return;
            }
            
            Logger.LogInformation("[Burned] START: ChainId={ChainId}, from={fromAddress}, amount={amount}",
                chainId, fromAddress, amount);
            await SaveUserBalanceAsync(symbol, fromAddress, -amount, context);
            Logger.LogInformation("[Burned] FINISH: ChainId={ChainId}, from={fromAddress}, amount={amount}",
                chainId, fromAddress, amount);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Burned] FINISH: ChainId={ChainId}, from={fromAddress}, amount={amount}",
                chainId, fromAddress, amount);
            throw;
        }
    }
}