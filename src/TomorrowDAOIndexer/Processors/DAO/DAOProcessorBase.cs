using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.Processors.DAO;

public abstract class DAOProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
    
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.DAOContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.DAOContractAddressMainNetSideChain,
            _ => throw new Exception("Unknown chain id")
        };
    }
}