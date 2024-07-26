using AElf.CSharp.Core;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public abstract class AssociationProcessorBase<TEvent> : NetworkDaoProposalBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.AssociationContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.AssociationContractAddressMainNetSideChain,
            TomorrowDAOConst.MainChainId => TomorrowDAOConst.AssociationContractAddress,
            _ => throw new Exception("Unknown chain id")
        };
    }
}