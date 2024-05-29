using System.Globalization;
using AElf.Indexing.Elasticsearch;
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
    [Name("getVoteSchemes")]
    public static async Task<List<VoteSchemeIndexDto>> GetVoteSchemesAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteSchemeIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper, GetVoteSchemeInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteSchemeIndex>, QueryContainer>>();
        if (!string.IsNullOrWhiteSpace(input.ChainId))
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }
        QueryContainer Filter(QueryContainerDescriptor<VoteSchemeIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
        
        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<VoteSchemeIndex>, List<VoteSchemeIndexDto>>(result.Item2);
    }
    
    [Name("getVoteItems")]
    public static async Task<List<VoteItemIndexDto>> GetVoteItemIndexAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteItemIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteInfoInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteItemIndex>, QueryContainer>>();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }

        if (!input.VotingItemIds.IsNullOrEmpty())
        {
            mustQuery.Add(q => q.Terms(i
                => i.Field(f => f.VotingItemId).Terms(input.VotingItemIds)));
        }

        if (!input.DaoIds.IsNullOrEmpty())
        {
            mustQuery.Add(q => q.Terms(i
                => i.Field(f => f.DAOId).Terms(input.DaoIds)));
        }
    
        QueryContainer Filter(QueryContainerDescriptor<VoteItemIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<VoteItemIndex>, List<VoteItemIndexDto>>(result.Item2);
    }

    [Name("getVoteWithdrawn")]
    public static async Task<List<VoteWithdrawnIndexDto>> GetVoterWithdrawnIndexAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteWithdrawnIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper, VoteWithdrawnIndexInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteWithdrawnIndex>, QueryContainer>>();
        if (!string.IsNullOrWhiteSpace(input.ChainId))
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }

        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.DaoId).Value(input.DaoId)));
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.Voter).Value(input.Voter)));

        QueryContainer Filter(QueryContainerDescriptor<VoteWithdrawnIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        return objectMapper.Map<List<VoteWithdrawnIndex>, List<VoteWithdrawnIndexDto>>(await GetAllIndex(Filter, repository));
    }
    
    [Name("getAllVoteRecord")]
    public static async Task<List<VoteRecordDto>> GetAllVoteRecordAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteRecordIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetAllVoteRecordInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteRecordIndex>, QueryContainer>>
        {
            q => q.Term(i 
                => i.Field(f => f.ChainId).Value(input.ChainId)),
            q => q.Term(i 
            => i.Field(f => f.DAOId).Value(input.DAOId)),
            q => q.Term(i 
                => i.Field(f => f.Voter).Value(input.Voter))
        };
    
        QueryContainer Filter(QueryContainerDescriptor<VoteRecordIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        return objectMapper.Map<List<VoteRecordIndex>, List<VoteRecordDto>>(await GetAllIndex(Filter, repository));
    }
    
    [Name("getVoteRecordCount")]
    public static async Task<long> GetVoteRecordCountAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteRecordIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteRecordCountInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteRecordIndex>, QueryContainer>>();

        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }

        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.DAOId).Value(input.DaoId)));
        }
        if (!input.StartTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.StartTime, DateFormat, CultureInfo.InvariantCulture);
            mustQuery.Add(q => q.DateRange(i => i.Field(f => f.VoteTimestamp).GreaterThanOrEquals(dateTime)));
        }

        if (!input.EndTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.EndTime, DateFormat, CultureInfo.InvariantCulture);
            mustQuery.Add(q => q.DateRange(i => i.Field(f => f.VoteTimestamp).LessThanOrEquals(dateTime)));
        }

        QueryContainer Filter(QueryContainerDescriptor<VoteRecordIndex> f) => f.Bool(b => b.Must(mustQuery));

        var result = await repository.CountAsync(Filter);
        return result?.Count ?? 0;
    }
    
    private static async Task<List<T>> GetAllIndex<T>(Func<QueryContainerDescriptor<T>, QueryContainer> filter, 
        IAElfIndexerClientEntityRepository<T, LogEventInfo> repository) 
        where T : AElfIndexerClientEntity<string>, IIndexBuild, new()
    {
        var res = new List<T>();
        List<T> list;
        var skipCount = 0;
        
        do
        {
            list = (await repository.GetListAsync(filterFunc: filter, skip: skipCount, limit: 1000)).Item2;
            var count = list.Count;
            res.AddRange(list);
            if (list.IsNullOrEmpty() || count < 1000)
            {
                break;
            }
            skipCount += count;
        } while (!list.IsNullOrEmpty());

        return res;
    }
}