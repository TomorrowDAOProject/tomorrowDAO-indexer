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
    [Name("getTreasuryFundListAsync")]
    public static async Task<List<TreasuryFundDto>> GetTreasuryFundListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetChainBlockHeightInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<TreasuryFundIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));
    
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
    
        QueryContainer Filter(QueryContainerDescriptor<TreasuryFundIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
            sortType: SortOrder.Ascending, sortExp: o => o.BlockHeight);
        return objectMapper.Map<List<TreasuryFundIndex>, List<TreasuryFundDto>>(result.Item2);
    }
    
    [Name("getTreasuryRecordListAsync")]
    public static async Task<List<TreasuryRecordDto>> GetTreasuryRecordListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetChainBlockHeightInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<TreasuryRecordIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));
        
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
        QueryContainer Filter(QueryContainerDescriptor<TreasuryRecordIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
            sortType: SortOrder.Descending, sortExp: o => o.BlockHeight);
        return objectMapper.Map<List<TreasuryRecordIndex>, List<TreasuryRecordDto>>(result.Item2);
    }
}