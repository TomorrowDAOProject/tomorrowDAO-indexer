using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using GraphQL;
using Nest;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.GraphQL;

public partial class Query
{
    [Name("getSyncProposalInfos")]
    public static async Task<List<ProposalSyncDto>> GetSyncProposalInfosAsync(
        [FromServices] IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetChainBlockHeightInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<ProposalIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));
    
        if (input.StartBlockHeight > 0)
        {
            mustQuery.Add(q => q.Range(i
                => i.Field(f => f.BlockHeight).GreaterThanOrEquals(input.StartBlockHeight)));
        }
    
        if (input.EndBlockHeight > 0)
        {
            mustQuery.Add(q => q.Range(i
                => i.Field(f => f.BlockHeight).LessThanOrEquals(input.EndBlockHeight)));
        }
    
        QueryContainer Filter(QueryContainerDescriptor<ProposalIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetListAsync(Filter, skip: input.SkipCount,
            sortType: SortOrder.Ascending, sortExp: o => o.BlockHeight);
        return objectMapper.Map<List<ProposalIndex>, List<ProposalSyncDto>>(result.Item2);
    }

    [Name("getVoteInfosMemory")]
    public static async Task<List<VoteInfoDto>> GetVoteInfoMemoryAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteItemIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteInfoInput input)
    {
        if (input.VotingItemIds.IsNullOrEmpty())
        {
            return new List<VoteInfoDto>();
        }
    
        var tasks = input.VotingItemIds
            .Select(votingItemId => repository.GetFromBlockStateSetAsync(votingItemId, input.ChainId)).ToList();
    
        var results = await Task.WhenAll(tasks);
        return results.Where(index => index != null).Select(objectMapper.Map<VoteItemIndex, VoteInfoDto>)
            .ToList();
    }
    
    [Name("getVoteInfos")]
    public static async Task<List<VoteInfoDto>> GetVoteInfosAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteItemIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteInfoInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteItemIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));
    
        mustQuery.Add(q => q.Terms(i
            => i.Field(f => f.VotingItemId).Terms(input.VotingItemIds)));
    
        QueryContainer Filter(QueryContainerDescriptor<VoteItemIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<VoteItemIndex>, List<VoteInfoDto>>(result.Item2);
    }
    
    [Name("getVoteRecord")]
    public static async Task<List<VoteRecordDto>> GetVoteRecordAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteRecordIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteRecordInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteRecordIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));
    
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.VotingItemId).Value(input.VotingItemId)));
    
        if (!input.Voter.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.Voter).Value(input.Voter)));
        }
    
        QueryContainer Filter(QueryContainerDescriptor<VoteRecordIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var sortDescriptor = GetVoteRecordSort(input.Sorting);
        var result = await repository.GetSortListAsync(Filter, sortFunc: sortDescriptor);
        return objectMapper.Map<List<VoteRecordIndex>, List<VoteRecordDto>>(result.Item2);
    }
    
    private static Func<SortDescriptor<VoteRecordIndex>, IPromise<IList<ISort>>> GetVoteRecordSort(string sorting)
    {
        var sortDescriptor = new SortDescriptor<VoteRecordIndex>();
    
        if (sorting.IsNullOrWhiteSpace())
        {
            sortDescriptor.Descending(a => a.VoteTimestamp);
            return _ => sortDescriptor;
        }
    
        var sortingArray = sorting.Trim().ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var field = sortingArray[0];
        var order = sortingArray.Length == 1 ? TomorrowDAOConst.Asc : sortingArray[1];
    
        switch (field)
        {
            case TomorrowDAOConst.VoteTime:
                if (order == TomorrowDAOConst.Asc || order == TomorrowDAOConst.Ascend)
                {
                    sortDescriptor.Ascending(a => a.VoteTimestamp);
                }
                else
                {
                    sortDescriptor.Descending(a => a.VoteTimestamp);
                }
                break;
            case TomorrowDAOConst.Amount:
                if (order == TomorrowDAOConst.Asc || order == TomorrowDAOConst.Ascend)
                {
                    sortDescriptor.Ascending(a => a.Amount);
                }
                else
                {
                    sortDescriptor.Descending(a => a.Amount);
                }
                break;
            default:
                sortDescriptor.Descending(a => a.VoteTimestamp);
                break;
        }
        return _ => sortDescriptor;
    }
    
    [Name("getVoteSchemeInfo")]
    public static async Task<List<VoteSchemeInfoDto>> GetVoteSchemeInfoAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteSchemeIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteSchemeInput input)
    {
        var voteMechanismList = input.Types?.Where(x => Enum.IsDefined(typeof(VoteMechanism), x))
            .Select(x => (VoteMechanism)Enum.Parse(typeof(VoteMechanism), x.ToString())).ToList() ?? new List<VoteMechanism>();
        
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteSchemeIndex>, QueryContainer>>
        {
            q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)),
            q => q.Terms(i
                => i.Field(f => f.VoteMechanism).Terms(voteMechanismList))
        };
    
        QueryContainer Filter(QueryContainerDescriptor<VoteSchemeIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<VoteSchemeIndex>, List<VoteSchemeInfoDto>>(result.Item2);
    }
}