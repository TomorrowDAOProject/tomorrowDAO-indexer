using AElf.CSharp.Core;
using TomorrowDAOIndexer.Processors.Common;

namespace TomorrowDAOIndexer.Processors.Treasury;

public abstract class TreasuryProcessorBase<TEvent> : StatisticProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.TreasuryContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.TreasuryContractAddressMainNetSideChain,
            _ => string.Empty
        };
    }
}