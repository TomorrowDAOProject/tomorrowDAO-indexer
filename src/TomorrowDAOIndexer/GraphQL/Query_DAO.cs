using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Dto;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getDaoList")]
    public static async Task<List<DAOInfoDto>> DAOIndex(
        [FromServices] IReadOnlyRepository<DAOIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetDaoListInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        var accounts= queryable.ToList();
        return objectMapper.Map<List<DAOIndex>, List<DAOInfoDto>>(accounts);
    }
}