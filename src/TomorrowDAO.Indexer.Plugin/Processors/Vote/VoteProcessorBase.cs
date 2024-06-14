using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Vote;

public abstract class VoteProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IVoteProvider VoteProvider;
    protected readonly IDAOProvider _daoProvider;
    protected readonly IAElfIndexerClientEntityRepository<LatestParticipatedIndex, LogEventInfo> LatestParticipatedRepository;

    protected VoteProcessorBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<LatestParticipatedIndex, LogEventInfo> latestParticipatedRepository, 
        IVoteProvider voteProvider, IDAOProvider daoProvider) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        LatestParticipatedRepository = latestParticipatedRepository;
        VoteProvider = voteProvider;
        _daoProvider = daoProvider;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].VoteContractAddress;
    }
    
    protected async Task SaveIndexAsync(LatestParticipatedIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await LatestParticipatedRepository.AddOrUpdateAsync(index);
    }
}