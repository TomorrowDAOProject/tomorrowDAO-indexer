using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using GraphQL;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.GraphQL;

public partial class Query
{
    [Name("GetVoteSchemes")]
    public static async Task<List<VoteSchemeIndexDto>> GetVoteSchemesAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteSchemeIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper, GovernanceSchemeIndexInput input)
    {
        var result = await repository.GetListAsync();
        return objectMapper.Map<List<VoteSchemeIndex>, List<VoteSchemeIndexDto>>(result.Item2);
    }
}