using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Serilog;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IVoteProvider
{
    Task<VoteSchemeIndex> GetVoteSchemeAsync(string chainId, string voteSchemeId);
    
    Task<VoteItemIndex> GetVoteItemAsync(string chainId, string votingItemId);
    
    Task<VoteRecordIndex> GetVoteRecordAsync(string chainId, string voteId);

    Task<VoteWithdrawnIndex> GetVoteWithdrawnAsync(string chainId, string id);

    Task SaveVoteSchemeIndexAsync(VoteSchemeIndex index, LogEventContext context);

    Task SaveVoteItemIndexAsync(VoteItemIndex itemIndex, LogEventContext context);

    Task SaveVoteRecordIndexAsync(VoteRecordIndex index, LogEventContext context);
    
    Task SaveVoteWithdrawnAsync(VoteWithdrawnIndex voteWithdrawn, LogEventContext context);
}

public class VoteProvider : IVoteProvider, ISingletonDependency
{
    private readonly ILogger<VoteProvider> _logger;
    private readonly IAElfIndexerClientEntityRepository<VoteSchemeIndex, LogEventInfo> _voteSchemeRepository;
    private readonly IAElfIndexerClientEntityRepository<VoteItemIndex, LogEventInfo> _voteItemRepository;
    private readonly IAElfIndexerClientEntityRepository<VoteRecordIndex, LogEventInfo> _voteRecordRepository;
    private readonly IAElfIndexerClientEntityRepository<VoteWithdrawnIndex, LogEventInfo> _voteWithdrawnRepository;
    private readonly IObjectMapper _objectMapper;
    
    public VoteProvider(ILogger<VoteProvider> logger, 
        IAElfIndexerClientEntityRepository<VoteSchemeIndex, LogEventInfo> voteSchemeRepository,
        IAElfIndexerClientEntityRepository<VoteItemIndex, LogEventInfo> voteItemRepository,
        IAElfIndexerClientEntityRepository<VoteRecordIndex, LogEventInfo> voteRecordRepository,
        IAElfIndexerClientEntityRepository<VoteWithdrawnIndex, LogEventInfo> voteWithdrawnRepository,
        IObjectMapper objectMapper)
    {
        _logger = logger;
        _voteSchemeRepository = voteSchemeRepository;
        _voteItemRepository = voteItemRepository;
        _voteRecordRepository = voteRecordRepository;
        _voteWithdrawnRepository = voteWithdrawnRepository;
        _objectMapper = objectMapper;
    }

    public async Task<VoteSchemeIndex> GetVoteSchemeAsync(string chainId, string voteSchemeId)
    {
        var voteSchemeIndex = await _voteSchemeRepository.GetFromBlockStateSetAsync(voteSchemeId, chainId);
        if (voteSchemeIndex != null)
        {
            return voteSchemeIndex;
        }
        _logger.LogInformation("VoteSchemeIndex with id {id} chainId {chainId} not existed.", voteSchemeId, chainId);
        return null;
    }

    public async Task<VoteItemIndex> GetVoteItemAsync(string chainId, string votingItemId)
    {
        var voteItemIndex = await _voteItemRepository.GetFromBlockStateSetAsync(votingItemId, chainId);
        if (voteItemIndex != null)
        {
            return voteItemIndex;
        }
        _logger.LogInformation("VoteItemIndex with id {id} chainId {chainId} not existed.", votingItemId, chainId);
        return null;
    }
    
    public async Task<VoteRecordIndex> GetVoteRecordAsync(string chainId, string voteId)
    {
        var voteRecordIndex = await _voteRecordRepository.GetFromBlockStateSetAsync(voteId, chainId);
        if (voteRecordIndex != null)
        {
            return voteRecordIndex;
        }
        _logger.LogInformation("VoteRecordIndex with id {id} chainId {chainId} not existed.", voteId, chainId);
        return null;
    }

    public async Task<VoteWithdrawnIndex> GetVoteWithdrawnAsync(string chainId, string id)
    {
        var voteWithdrawnIndex = await _voteWithdrawnRepository.GetFromBlockStateSetAsync(id, chainId);
        if (voteWithdrawnIndex != null)
        {
            return voteWithdrawnIndex;
        }
        _logger.LogInformation("VoteWithdrawnIndexDto with id {id} chainId {chainId} not existed.", id, chainId);
        return null;
    }

    public async Task SaveVoteSchemeIndexAsync(VoteSchemeIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _voteSchemeRepository.AddOrUpdateAsync(index);
    }
    
    public async Task SaveVoteItemIndexAsync(VoteItemIndex itemIndex, LogEventContext context)
    {
        _objectMapper.Map(context, itemIndex);
        await _voteItemRepository.AddOrUpdateAsync(itemIndex);
    }
    
    public async Task SaveVoteRecordIndexAsync(VoteRecordIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _voteRecordRepository.AddOrUpdateAsync(index);
    }

    public async Task SaveVoteWithdrawnAsync(VoteWithdrawnIndex voteWithdrawn, LogEventContext context)
    {
        _objectMapper.Map(context, voteWithdrawn);
        await _voteWithdrawnRepository.AddOrUpdateAsync(voteWithdrawn);
    }
}