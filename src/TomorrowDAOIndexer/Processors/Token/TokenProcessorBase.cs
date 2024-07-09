using AElf.CSharp.Core;
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
}