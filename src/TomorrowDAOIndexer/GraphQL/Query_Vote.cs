using System.Globalization;
using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
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
    
    // todo server change
    // dto int to long
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
            queryable = queryable.Where(
                input.VotingItemIds.Select(votingItemId => (Expression<Func<VoteItemIndex, bool>>)(o => o.VotingItemId == votingItemId))
                    .Aggregate((prev, next) => prev.Or(next)));
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
    public static async Task<GetVoteRecordCountDto> GetVoteRecordCountAsync(
        [FromServices] IReadOnlyRepository<VoteRecordIndex> repository, GetVoteRecordCountInput input)
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

        return new GetVoteRecordCountDto { Count = queryable.Count() };
    }
    
    [Name("getLimitVoteRecord")]
    public static async Task<List<VoteRecordDto>> GetLimitVoteRecordAsync(
        [FromServices] IReadOnlyRepository<VoteRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetLimitVoteRecordInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Where(a => a.VotingItemId == input.VotingItemId);
    
        if (!input.Voter.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Voter == input.Voter);
        }
    
        if (input.Limit == 0)
        {
            input.Limit = 1;
        }
    
        List<VoteRecordIndex> result;
        if (input.Sorting.IsNullOrEmpty())
        {
            result = queryable.Skip(0).Take(input.Limit)
                .OrderByDescending(a => a.VoteTimestamp).ToList();
        }
        else
        {
            result = queryable.Skip(0).Take(input.Limit)
                .OrderByDescending(a => a.Amount).ToList();
        }
    
        return objectMapper.Map<List<VoteRecordIndex>, List<VoteRecordDto>>(result);
    }
    
    // todo server change
    // TryParse change
    [Name("getPageVoteRecord")]
    public static async Task<List<VoteRecordDto>> GetPageVoteRecordAsync(
        [FromServices] IReadOnlyRepository<VoteRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetPageVoteRecordInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Where(a => a.Voter == input.Voter)
            .Where(a => a.Option == input.VoteOption);
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DAOId == input.DaoId);
        }
        if (!input.VotingItemId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.VotingItemId == input.VotingItemId);
        }
        
        var result = queryable
            .Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderByDescending(a => a.VoteTimestamp).ToList();
        
        return objectMapper.Map<List<VoteRecordIndex>, List<VoteRecordDto>>(result);
    }
    
    [Name("getVoteSchemeInfo")]
    public static async Task<List<VoteSchemeInfoDto>> GetVoteSchemeInfoAsync(
        [FromServices] IReadOnlyRepository<VoteSchemeIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetVoteSchemeInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        return objectMapper.Map<List<VoteSchemeIndex>, List<VoteSchemeInfoDto>>(queryable.ToList());
    }
}