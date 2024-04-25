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
    [Name("getGovernanceSchemeIndex")]
    public static async Task<List<GovernanceSchemeIndexDto>> GetGovernanceSchemeAsync(
        [FromServices] IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper, GovernanceSchemeIndexInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<GovernanceSchemeIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.DAOId).Value(input.DAOId)));

        QueryContainer Filter(QueryContainerDescriptor<GovernanceSchemeIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<GovernanceSchemeIndex>, List<GovernanceSchemeIndexDto>>(result.Item2);
    }
}