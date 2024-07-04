using AElf.CSharp.Core;

namespace TomorrowDAOIndexer.Processors.Election;

public abstract class ElectionProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected const int CandidateTerm = 0;
    
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.ElectionContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.ElectionContractAddressMainNetSideChain,
            _ => throw new Exception("Unknown chain id")
        };
    }
}