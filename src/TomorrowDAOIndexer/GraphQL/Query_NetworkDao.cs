using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getNetworkDaoProposals")]
    public static async Task<NetworkDaoProposalsDto> GetNetworkDaoProposalsAsync(
        [FromServices] IReadOnlyRepository<NetworkDaoProposalIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoProposalsInput input)
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
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (input.ProposalType != NetworkDaoProposalType.All)
        {
            queryable = queryable.Where(a => a.ProposalType == input.ProposalType);
        }
        if (!input.ProposalIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.ProposalIds.Select(proposalId => (Expression<Func<NetworkDaoProposalIndex, bool>>)(o => o.ProposalId == proposalId))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        // var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
        //     sortType: SortOrder.Ascending, sortExp: o => o.BlockHeight);
        //
        // return new NetworkDaoProposalsDto
        // {
        //     TotalCount = result.Item1,
        //     Items = objectMapper.Map<List<NetworkDaoProposalIndex>, List<NetworkDaoProposal>>(result.Item2)
        // };
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        return new NetworkDaoProposalsDto
        {
            TotalCount = count,
            Items = objectMapper.Map<List<NetworkDaoProposalIndex>, List<NetworkDaoProposal>>(queryable.ToList())
        };
    }
}