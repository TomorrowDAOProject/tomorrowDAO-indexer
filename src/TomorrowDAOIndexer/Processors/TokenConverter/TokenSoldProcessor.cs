using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.TokenConverter;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.TokenConverter;

public class TokenSoldProcessor : TokenConverterProcessorBase<TokenSold>
{
    public override async Task ProcessAsync(TokenSold logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var feeAmount = logEvent.FeeAmount;
        var baseAmount = logEvent.BaseAmount;
        var soldAmount = logEvent.SoldAmount;
        Logger.LogInformation("[TokenBought] start chainId:{proposalId} feeAmount:{chainId} baseAmount {baseAmount} soldAmount {soldAmount}",
            chainId, feeAmount, baseAmount, soldAmount);
        try
        {
            await SaveEntityAsync(new ResourceTokenIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId),
                TransactionId = context.Transaction.TransactionId,
                Symbol = logEvent.Symbol,
                Method = TomorrowDAOConst.TokenConverterContractAddressSellMethod,
                ResourceAmount = soldAmount,
                BaseAmount = baseAmount,
                FeeAmount = feeAmount,
                ChainId = chainId,
                BlockHeight = context.Block.BlockHeight,
                TransactionStatus = context.Transaction.Status.ToString(),
                OperateTime = context.Block.BlockTime
            });
            Logger.LogInformation("[TokenBought] start chainId:{proposalId} feeAmount:{chainId} baseAmount {baseAmount} soldAmount {soldAmount}",
                chainId, feeAmount, baseAmount, soldAmount);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TokenBought] start chainId:{proposalId} feeAmount:{chainId} baseAmount {baseAmount} soldAmount {soldAmount}",
                chainId, feeAmount, baseAmount, soldAmount);
            throw;
        }
    }
}