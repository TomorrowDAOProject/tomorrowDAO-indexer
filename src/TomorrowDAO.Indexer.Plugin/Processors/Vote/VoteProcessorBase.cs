using AElf.CSharp.Core;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Vote;

public abstract class VoteProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly ILogger<VoteProcessorBase<TEvent>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IVoteProvider VoteProvider;
    
    protected VoteProcessorBase(ILogger<VoteProcessorBase<TEvent>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IVoteProvider voteProvider) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        VoteProvider = voteProvider;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].VoteContractAddress;
    }
}