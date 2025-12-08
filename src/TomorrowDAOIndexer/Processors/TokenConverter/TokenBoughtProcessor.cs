using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.TokenConverter;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.TokenConverter;

public class TokenBoughtProcessor : TokenConverterProcessorBase<TokenBought>
{
    public override async Task ProcessAsync(TokenBought logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var feeAmount = logEvent.FeeAmount;
        var baseAmount = logEvent.BaseAmount;
        var boughtAmount = logEvent.BoughtAmount;
        Logger.LogInformation("[TokenBought] start chainId:{proposalId} feeAmount:{chainId} baseAmount {baseAmount} boughtAmount {boughtAmount}",
            chainId, feeAmount, baseAmount, boughtAmount);
        try
        {
            await SaveEntityAsync(new ResourceTokenIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId),
                TransactionId = context.Transaction.TransactionId,
                Symbol = logEvent.Symbol,
                Method = TomorrowDAOConst.TokenConverterContractAddressBuyMethod,
                ResourceAmount = boughtAmount,
                BaseAmount = baseAmount,
                FeeAmount = feeAmount,
                ChainId = chainId,
                BlockHeight = context.Block.BlockHeight,
                TransactionStatus = context.Transaction.Status.ToString(),
                OperateTime = context.Block.BlockTime
            }, context);
            Logger.LogInformation("[TokenBought] start chainId:{proposalId} feeAmount:{chainId} baseAmount {baseAmount} boughtAmount {boughtAmount}",
                chainId, feeAmount, baseAmount, boughtAmount);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TokenBought] start chainId:{proposalId} feeAmount:{chainId} baseAmount {baseAmount} boughtAmount {boughtAmount}",
                chainId, feeAmount, baseAmount, boughtAmount);
            throw;
        }
    }
}