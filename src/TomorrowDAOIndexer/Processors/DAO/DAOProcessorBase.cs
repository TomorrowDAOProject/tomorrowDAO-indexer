using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.Processors.DAO;

public abstract class DAOProcessorBase<TEvent> : LogEventProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();
    
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            "tDVW" => "RRF7deQbmicUh6CZ1R2y7U9M8n2eHPyCgXVHwiSkmNETLbL4D",
            "tDVV" => "2izSidAeMiZ6tmD7FKmnoWbygjFSmH5nko3cGJ9EtbfC44BycC",
            _ => throw new Exception("Unknown chain id")
        };
    }
}