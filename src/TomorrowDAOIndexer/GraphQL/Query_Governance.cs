using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getGovernanceSchemeIndex")]
    public static async Task<List<GovernanceSchemeIndexDto>> GetGovernanceSchemeAsync(
        [FromServices] IReadOnlyRepository<GovernanceSchemeIndex> repository,
        [FromServices] IObjectMapper objectMapper, GovernanceSchemeIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }

        if (!input.DAOId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DAOId == input.DAOId);
        }
        
        return objectMapper.Map<List<GovernanceSchemeIndex>, List<GovernanceSchemeIndexDto>>(queryable.ToList());
    }
}