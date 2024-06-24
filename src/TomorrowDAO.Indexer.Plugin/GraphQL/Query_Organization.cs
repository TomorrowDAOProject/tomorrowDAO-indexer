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
    [Name("getMemberListAsync")]
    public static async Task<MemberListPageResultDto> GetMemberListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetMemberListInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<OrganizationIndex>, QueryContainer>>
        {
            q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)),
            q => q.Term(i
                => i.Field(f => f.DAOId).Value(input.DAOId))
        };
    
        QueryContainer Filter(QueryContainerDescriptor<OrganizationIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetSortListAsync(Filter,
            sortFunc: s => s.Ascending(a => a.CreateTime),
            skip: input.SkipCount, limit: input.MaxResultCount);
        return new MemberListPageResultDto
        {
            TotalCount = result.Item1,
            DataList = objectMapper.Map<List<OrganizationIndex>, List<MemberDto>>(result.Item2)
        };
    }
}