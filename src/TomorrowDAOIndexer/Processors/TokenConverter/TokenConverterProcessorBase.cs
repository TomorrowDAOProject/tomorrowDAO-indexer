using AElf.CSharp.Core;
using TomorrowDAOIndexer.Processors.Common;

namespace TomorrowDAOIndexer.Processors.TokenConverter;

public abstract class TokenConverterProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.MainChainId => TomorrowDAOConst.TokenConverterContractAddress,
            _ => string.Empty
        };
    }
}