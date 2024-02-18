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
    [Name("getOrganizationInfosMemory")]
    public static async Task<List<OrganizationInfoDto>> GetOrganizationInfosMemoryAsync(
        [FromServices] IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetOrganizationInfoInput input)
    {
        if (input.OrganizationAddressList.IsNullOrEmpty())
        {
            return new List<OrganizationInfoDto>();
        }

        var tasks = input.OrganizationAddressList
            .Select(organizationAddress => repository.GetFromBlockStateSetAsync(organizationAddress, input.ChainId))
            .ToList();

        var results = await Task.WhenAll(tasks);
        return results.Where(index => index != null).Select(objectMapper.Map<OrganizationIndex, OrganizationInfoDto>)
            .ToList();
    }

    [Name("getOrganizationInfos")]
    public static async Task<List<OrganizationInfoDto>> GetOrganizationInfosAsync(
        [FromServices] IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetOrganizationInfoInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<OrganizationIndex>, QueryContainer>>();

        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));

        if (!input.OrganizationAddressList.IsNullOrEmpty())
        {
            mustQuery.Add(q => q.Terms(i
                => i.Field(f => f.OrganizationAddress).Terms(input.OrganizationAddressList)));
        }

        if (!input.GovernanceSchemeId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.GovernanceSchemeId).Value(input.GovernanceSchemeId)));
        }

        QueryContainer Filter(QueryContainerDescriptor<OrganizationIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter);

        return objectMapper.Map<List<OrganizationIndex>, List<OrganizationInfoDto>>(result.Item2);
    }
}