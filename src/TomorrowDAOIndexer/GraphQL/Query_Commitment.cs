using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getCommitments")]
    public static async Task<List<CommitmentDto>> GetCommitmentsAsync(
        [FromServices] IReadOnlyRepository<CommitmentIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetChainBlockHeightInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }

        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }

        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        var result = queryable.ToList();
        return objectMapper.Map<List<CommitmentIndex>, List<CommitmentDto>>(result);
    }

    [Name("getLimitCommitments")]
    public static async Task<List<CommitmentDto>> GetLimitCommitmentsAsync(
        [FromServices] IReadOnlyRepository<CommitmentIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetLimitCommitmentInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Where(a => a.ProposalId == input.VotingItemId);

        if (!input.Voter.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Voter == input.Voter);
        }

        if (input.Limit == 0)
        {
            input.Limit = 1;
        }

        List<CommitmentIndex> result;
        if (input.Sorting.IsNullOrEmpty())
        {
            result = queryable.Skip(0).Take(input.Limit)
                .OrderByDescending(a => a.Timestamp).ToList();
        }
        else
        {
            result = queryable.Skip(0).Take(input.Limit)
                .OrderByDescending(a => a.Timestamp).ToList();
        }

        return objectMapper.Map<List<CommitmentIndex>, List<CommitmentDto>>(result);
    }
}