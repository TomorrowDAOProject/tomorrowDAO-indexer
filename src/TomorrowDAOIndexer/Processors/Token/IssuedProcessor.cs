using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.MultiToken;

namespace TomorrowDAOIndexer.Processors.Token;

public class IssuedProcessor : TokenProcessorBase<Issued>
{
    public override async Task ProcessAsync(Issued logEvent, LogEventContext context)
    {
        var toAddress = logEvent.To.ToBase58();
        var amount = logEvent.Amount;
        var symbol = logEvent.Symbol;
        var chainId = context.ChainId;
        try
        {
            if (!CheckSymbol(chainId, symbol))
            {
                return;
            }
            
            Logger.LogInformation("[Burned] START: ChainId={ChainId}, to={toAddress}, amount={amount}",
                chainId, toAddress, amount);
            await SaveUserBalanceAsync(symbol, toAddress, amount, context);
            Logger.LogInformation("[Burned] FINISH: ChainId={ChainId}, to={toAddress}, amount={amount}",
                chainId, toAddress, amount);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Burned] FINISH: ChainId={ChainId}, to={toAddress}, amount={amount}",
                chainId, toAddress, amount);
            throw;
        }
    }
}