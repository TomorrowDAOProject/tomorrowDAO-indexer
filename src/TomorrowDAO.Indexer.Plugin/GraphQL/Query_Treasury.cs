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
    [Name("getTreasuryFundList")]
    public static async Task<Tuple<long, List<TreasuryFundDto>>> GetTreasuryFundListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetTreasuryFundListInput input)
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

        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }

        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.DaoId).Value(input.DaoId)));
        }

        if (!input.TreasuryAddress.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.TreasuryAddress).Value(input.TreasuryAddress)));
        }

        if (!input.Symbols.IsNullOrEmpty())
        {
            mustQuery.Add(q => q.Terms(i
                => i.Field(f => f.Symbol).Terms(input.Symbols)));
        }

        QueryContainer Filter(QueryContainerDescriptor<TreasuryFundIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
            sortType: SortOrder.Ascending, sortExp: o => o.AvailableFunds);
        return new Tuple<long, List<TreasuryFundDto>>(result.Item1,
            objectMapper.Map<List<TreasuryFundIndex>, List<TreasuryFundDto>>(result.Item2));
    }

    [Name("getTreasuryRecordList")]
    public static async Task<List<TreasuryRecordDto>> GetTreasuryRecordListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetTreasuryFundListInput input)
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

        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }

        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.DaoId).Value(input.DaoId)));
        }

        if (!input.TreasuryAddress.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.TreasuryAddress).Value(input.TreasuryAddress)));
        }

        QueryContainer Filter(QueryContainerDescriptor<TreasuryRecordIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
            sortType: SortOrder.Descending, sortExp: o => o.BlockHeight);
        return objectMapper.Map<List<TreasuryRecordIndex>, List<TreasuryRecordDto>>(result.Item2);
    }
}