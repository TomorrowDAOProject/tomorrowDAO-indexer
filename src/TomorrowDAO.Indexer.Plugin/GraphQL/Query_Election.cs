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
    [Name("getElectionListAsync")]
    public static async Task<List<ElectionDto>> GetElectionListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetChainBlockHeightInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<ElectionIndex>, QueryContainer>>();
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

        QueryContainer Filter(QueryContainerDescriptor<ElectionIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
            sortType: SortOrder.Ascending, sortExp: o => o.BlockHeight);
        return objectMapper.Map<List<ElectionIndex>, List<ElectionDto>>(result.Item2);
    }

    [Name("getHighCouncilListAsync")]
    public static async Task<ElectionListPageResultDto> GetHighCouncilListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetHighCouncilListInput input)
    {
        if (!Enum.TryParse(input.HighCouncilType, out HighCouncilType highCouncilType))
        {
            return new ElectionListPageResultDto
            {
                TotalCount = 0,
                DataList = new List<ElectionDto>()
            };
        }
        
        var mustQuery = new List<Func<QueryContainerDescriptor<ElectionIndex>, QueryContainer>>
        {
            q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)),
            q => q.Term(i
                => i.Field(f => f.TermNumber).Value(input.TermNumber)),
            q => q.Term(i
                => i.Field(f => f.HighCouncilType).Value(highCouncilType))
        };

        if (!string.IsNullOrEmpty(input.DAOId))
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.DAOId).Value(input.DAOId)));
        }
        
        QueryContainer Filter(QueryContainerDescriptor<ElectionIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
        
        var result = await repository.GetListAsync(Filter, skip: input.SkipCount, limit: input.MaxResultCount,
            sortType: SortOrder.Descending, sortExp: o => o.BlockHeight);
        return new ElectionListPageResultDto
        {
            TotalCount = result.Item1,
            DataList = objectMapper.Map<List<ElectionIndex>, List<ElectionDto>>(result.Item2)
        };
    }
}