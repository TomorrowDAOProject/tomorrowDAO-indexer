using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IElectionProvider
{
    Task<ElectionVotingItemIndex> GetVotingItemIndexAsync(string id, string chainId);
    Task<ElectionHighCouncilConfigIndex> GetHighCouncilConfigIndexAsync(string id, string chainId);
    Task SaveIndexAsync(ElectionIndex index, LogEventContext context);

    Task SaveVotingItemIndexAsync(ElectionVotingItemIndex votingItemIndex, LogEventContext context);
    Task SaveHighCouncilConfigIndexAsync(ElectionHighCouncilConfigIndex highCouncilConfigIndex, LogEventContext context);
}

public class ElectionProvider : IElectionProvider, ISingletonDependency
{
    private readonly ILogger<VoteProvider> _logger;
    private readonly IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> _electionRepository;
    private readonly IAElfIndexerClientEntityRepository<ElectionVotingItemIndex, LogEventInfo> _votingItemRepository;

    private readonly IAElfIndexerClientEntityRepository<ElectionHighCouncilConfigIndex, LogEventInfo>
        _highCouncilConfigRepository;

    private readonly IObjectMapper _objectMapper;

    public ElectionProvider(IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<ElectionVotingItemIndex, LogEventInfo> votingItemRepository,
        IAElfIndexerClientEntityRepository<ElectionHighCouncilConfigIndex, LogEventInfo> highCouncilConfigRepository,
        ILogger<VoteProvider> logger)
    {
        _electionRepository = electionRepository;
        _objectMapper = objectMapper;
        _votingItemRepository = votingItemRepository;
        _highCouncilConfigRepository = highCouncilConfigRepository;
        _logger = logger;
    }

    public async Task<ElectionVotingItemIndex> GetVotingItemIndexAsync(string id, string chainId)
    {
        var voteItemIndex = await _votingItemRepository.GetFromBlockStateSetAsync(id, chainId);
        if (voteItemIndex != null)
        {
            return voteItemIndex;
        }

        _logger.LogInformation("ElectionVoteItemIndex with id {id} chainId {chainId} not existed.", id, chainId);
        return null;
    }

    public async Task<ElectionHighCouncilConfigIndex> GetHighCouncilConfigIndexAsync(string id, string chainId)
    {
        var highCouncilConfig = await _highCouncilConfigRepository.GetFromBlockStateSetAsync(id, chainId);
        if (highCouncilConfig != null)
        {
            return highCouncilConfig;
        }

        _logger.LogInformation("ElectionHighCouncilConfigIndex with id {id} chainId {chainId} not existed.", id, chainId);
        return null;
    }

    public async Task SaveIndexAsync(ElectionIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _electionRepository.AddOrUpdateAsync(index);
    }

    public async Task SaveVotingItemIndexAsync(ElectionVotingItemIndex votingItemIndex, LogEventContext context)
    {
        _objectMapper.Map(context, votingItemIndex);
        await _votingItemRepository.AddOrUpdateAsync(votingItemIndex);
    }

    public async Task SaveHighCouncilConfigIndexAsync(ElectionHighCouncilConfigIndex highCouncilConfigIndex, LogEventContext context)
    {
        _objectMapper.Map(context, highCouncilConfigIndex);
        await _highCouncilConfigRepository.AddOrUpdateAsync(highCouncilConfigIndex);
    }
}