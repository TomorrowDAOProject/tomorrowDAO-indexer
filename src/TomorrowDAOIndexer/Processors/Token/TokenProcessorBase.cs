using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Processors.Common;

namespace TomorrowDAOIndexer.Processors.Token;

public abstract class TokenProcessorBase<TEvent> : StatisticProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.TokenContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.TokenContractAddressMainNetSideChain,
            _ => string.Empty
        };
    }

    protected async Task SaveUserBalanceAsync(string symbol, string address, long deltaAmount, LogEventContext context)
    {
        var userBalanceId = IdGenerateHelper.GetId(address, context.ChainId, symbol);
        var userBalanceIndex = await GetEntityAsync<UserBalanceIndex>(userBalanceId);
        if (userBalanceIndex == null)
        {
            userBalanceIndex = new UserBalanceIndex
            {
                Id = userBalanceId,
                Address = address,
                Amount = deltaAmount,
                Symbol = symbol,
                ChangeTime = context.Block.BlockTime,
                BlockHeight = context.Block.BlockHeight
            };
        }
        else
        {
            userBalanceIndex.Amount += deltaAmount;
            userBalanceIndex.ChangeTime = context.Block.BlockTime;
            userBalanceIndex.BlockHeight = context.Block.BlockHeight;
        }

        await SaveEntityAsync(userBalanceIndex);
    }

    protected bool CheckSymbol(string chainId, string symbol)
    {
        var collectionSymbol = chainId switch
        {
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.VotigramCollectionSymbolMainNet,
            TomorrowDAOConst.MainChainId => string.Empty,
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.VotigramCollectionSymbolTestNet,
            _ => string.Empty
        };
        return !string.IsNullOrEmpty(collectionSymbol) && symbol.StartsWith(collectionSymbol);
    }
}