using AElf.CSharp.Core;
using TomorrowDAOIndexer.Processors.Common;

namespace TomorrowDAOIndexer.Processors.DAO;

public abstract class DAOProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.DAOContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.DAOContractAddressMainNetSideChain,
            _ => string.Empty
        };
    }
}