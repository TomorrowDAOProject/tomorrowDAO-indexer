using AElf.CSharp.Core;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public abstract class GovernanceSchemeProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.GovernanceContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.GovernanceContractAddressMainNetSideChain,
            _ => throw new Exception("Unknown chain id")
        };
    }
}