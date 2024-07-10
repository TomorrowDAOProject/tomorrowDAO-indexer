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
    [Name("getNetworkDaoProposals")]
    public static async Task<NetworkDaoProposalsDto> GetNetworkDaoProposalsAsync(
        [FromServices] IAElfIndexerClientEntityRepository<NetworkDaoProposalIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetNetworkDaoProposalsInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<NetworkDaoProposalIndex>, QueryContainer>>();
        
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

        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }

        if (input.ProposalType != NetworkDaoProposalType.All)
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ProposalType).Value(input.ProposalType)));
        }

        if (!input.ProposalIds.IsNullOrEmpty())
        {
            mustQuery.Add(q => q.Terms(i
                => i.Field(f => f.ProposalId).Terms(input.ProposalIds)));
        }
        
        QueryContainer Filter(QueryContainerDescriptor<NetworkDaoProposalIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
            sortType: SortOrder.Ascending, sortExp: o => o.BlockHeight);

        return new NetworkDaoProposalsDto
        {
            TotalCount = result.Item1,
            Items = objectMapper.Map<List<NetworkDaoProposalIndex>, List<NetworkDaoProposal>>(result.Item2)
        };
    }
}