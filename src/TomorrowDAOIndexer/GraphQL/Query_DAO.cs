using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Dto;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getDAOList")]
    public static async Task<List<DAOInfoDto>> GetDAOListAsync(
        [FromServices] IReadOnlyRepository<DAOIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetChainBlockHeightInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.Metadata.Block.BlockHeight <= input.EndBlockHeight);
        }
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.Metadata.Block.BlockHeight) ;
        
        var accounts= queryable.ToList();
        return objectMapper.Map<List<DAOIndex>, List<DAOInfoDto>>(accounts);
    }
}