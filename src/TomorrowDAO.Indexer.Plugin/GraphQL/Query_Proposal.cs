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
    [Name("getSyncProposalInfos")]
    public static async Task<List<ProposalSyncDto>> GetSyncProposalInfosAsync(
        [FromServices] IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetChainBlockHeightInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<ProposalIndex>, QueryContainer>>();
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

        QueryContainer Filter(QueryContainerDescriptor<ProposalIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter, skip: input.SkipCount,
            sortType: SortOrder.Ascending, sortExp: o => o.BlockHeight);
        return objectMapper.Map<List<ProposalIndex>, List<ProposalSyncDto>>(result.Item2);
    }

    [Name("getVoteInfosMemory")]
    public static async Task<List<VoteInfoDto>> GetVoteInfoMemoryAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteInfoInput input)
    {
        if (input.VotingItemIds.IsNullOrEmpty())
        {
            return new List<VoteInfoDto>();
        }

        var tasks = input.VotingItemIds
            .Select(votingItemId => repository.GetFromBlockStateSetAsync(votingItemId, input.ChainId)).ToList();

        var results = await Task.WhenAll(tasks);
        return results.Where(index => index != null).Select(objectMapper.Map<VoteIndex, VoteInfoDto>)
            .ToList();
    }

    [Name("getVoteInfos")]
    public static async Task<List<VoteInfoDto>> GetVoteInfosAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteInfoInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<VoteIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));

        mustQuery.Add(q => q.Terms(i
            => i.Field(f => f.ChainId).Terms(input.VotingItemIds)));

        QueryContainer Filter(QueryContainerDescriptor<VoteIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var result = await repository.GetListAsync(Filter);
        return objectMapper.Map<List<VoteIndex>, List<VoteInfoDto>>(result.Item2);
    }
    
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
            .Select(organizationAddress => repository.GetFromBlockStateSetAsync(organizationAddress, input.ChainId)).ToList();

        var results = await Task.WhenAll(tasks);
        return results.Where(index => index != null).Select(objectMapper.Map<OrganizationIndex, OrganizationInfoDto>)
            .ToList();
    }
}