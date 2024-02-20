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
    [Name("getGovernanceModesAsync")]
    public static async Task<List<GovernanceMode>> GetGovernanceModesAsync(
        [FromServices] IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GovernanceModeInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<GovernanceSubSchemeIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));

        QueryContainer Filter(QueryContainerDescriptor<GovernanceSubSchemeIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<GovernanceSubSchemeIndex>, List<GovernanceMode>>(result.Item2);
    }
}