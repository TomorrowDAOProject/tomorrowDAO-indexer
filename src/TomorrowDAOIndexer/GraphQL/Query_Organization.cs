using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getMemberList")]
    public static async Task<MemberListPageResultDto> GetMemberListAsync(
        [FromServices] IReadOnlyRepository<OrganizationIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetMemberListInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        if (!input.DAOId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DAOId == input.DAOId);
        }    
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.CreateTime);
        return new MemberListPageResultDto
        {
            TotalCount = count,
            Data = objectMapper.Map<List<OrganizationIndex>, List<MemberDto>>(queryable.ToList())
        };
    }
    
    [Name("getMember")]
    public static async Task<MemberDto> GetMemberAsync(
        [FromServices] IReadOnlyRepository<OrganizationIndex> repository,
        [FromServices] IObjectMapper objectMapper,
        GetMemberInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        var id = IdGenerateHelper.GetId(input.ChainId, input.DAOId, input.Address);
        queryable = queryable.Where(a => a.Id == id);
        queryable = queryable.Where(a => a.DAOId == input.DAOId);
        return objectMapper.Map<OrganizationIndex, MemberDto>(queryable.SingleOrDefault() ?? new OrganizationIndex());
    }
}