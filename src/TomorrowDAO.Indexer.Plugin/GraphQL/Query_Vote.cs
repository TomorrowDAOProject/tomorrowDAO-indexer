using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using GraphQL;
using Nest;
using TomorrowDAO.Indexer.Plugin.Entities;
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

    [Name("getVoteWithdrawn")]
    public static async Task<List<VoteWithdrawnIndexDto>> GetVoterWithdrawnIndexAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteWithdrawnIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper, VoteWithdrawnIndexInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteWithdrawnIndex>, QueryContainer>>();
        if (!string.IsNullOrWhiteSpace(input.ChainId))
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input)));
        }

        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.DaoId).Value(input.DaoId)));
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.Voter).Value(input.Voter)));

        QueryContainer Filter(QueryContainerDescriptor<VoteWithdrawnIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<VoteWithdrawnIndex>, List<VoteWithdrawnIndexDto>>(result.Item2);
    }
}