using AElf.CSharp.Core;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

public abstract class ParliamentProcessorBase<TEvent> : NetworkDaoProposalBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.ParliamentContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.ParliamentContractAddressMainNetSideChain,
            TomorrowDAOConst.MainChainId => TomorrowDAOConst.ParliamentContractAddress,
            _ => throw new Exception("Unknown chain id")
        };
    }
}