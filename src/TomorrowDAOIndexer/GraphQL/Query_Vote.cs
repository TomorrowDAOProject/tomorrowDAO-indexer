using System.Globalization;
using AeFinder.Sdk;
using GraphQL;
using Nest;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getVoteSchemes")]
    public static async Task<List<VoteSchemeIndexDto>> GetVoteSchemesAsync(
        [FromServices] IReadOnlyRepository<VoteSchemeIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetVoteSchemeInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        return objectMapper.Map<List<VoteSchemeIndex>, List<VoteSchemeIndexDto>>(queryable.ToList());
    }
    
    [Name("getVoteItems")]
    public static async Task<List<VoteItemIndexDto>> GetVoteItemIndexAsync(
        [FromServices] IReadOnlyRepository<VoteItemIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetVoteInfoInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.VotingItemIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(a => input.VotingItemIds.Contains(a.VotingItemId));
        }
        if (!input.DaoIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(a => input.DaoIds.Contains(a.DAOId));
        }
        return objectMapper.Map<List<VoteItemIndex>, List<VoteItemIndexDto>>(queryable.ToList());
    }
    
    [Name("getVoteWithdrawn")]
    public static async Task<List<VoteWithdrawnIndexDto>> GetVoterWithdrawnIndexAsync(
        [FromServices] IReadOnlyRepository<VoteWithdrawnIndex> repository,
        [FromServices] IObjectMapper objectMapper, VoteWithdrawnIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        queryable = queryable.Where(a => a.DaoId == input.DaoId)
            .Where(a => a.Voter == input.Voter);
        return objectMapper.Map<List<VoteWithdrawnIndex>, List<VoteWithdrawnIndexDto>>(queryable.ToList());
    }
    
    [Name("getAllVoteRecord")]
    public static async Task<List<VoteRecordDto>> GetAllVoteRecordAsync(
        [FromServices] IReadOnlyRepository<VoteRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetAllVoteRecordInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Where(a => a.DAOId == input.DAOId)
            .Where(a => a.Voter == input.Voter);
        return objectMapper.Map<List<VoteRecordIndex>, List<VoteRecordDto>>(queryable.ToList());
    }
    
    [Name("getVoteRecordCount")]
    public static async Task<long> GetVoteRecordCountAsync(
        [FromServices] IReadOnlyRepository<VoteRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetVoteRecordCountInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DAOId == input.DaoId);
        }
        if (!input.StartTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.StartTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
            queryable = queryable.Where(a => a.VoteTimestamp >= dateTime);
        }
        if (!input.EndTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.EndTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
            queryable = queryable.Where(a => a.VoteTimestamp <= dateTime);
        }
        return queryable.Count();
    }
}