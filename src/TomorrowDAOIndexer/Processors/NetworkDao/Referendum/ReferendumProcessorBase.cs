using AElf.CSharp.Core;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Referendum;

public abstract class ReferendumProcessorBase<TEvent> : NetworkDaoProposalBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.ReferendumContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.ReferendumContractAddressMainNetSideChain,
            TomorrowDAOConst.MainChainId => TomorrowDAOConst.ReferendumContractAddress,
            _ => throw new Exception("Unknown chain id")
        };
    }
}