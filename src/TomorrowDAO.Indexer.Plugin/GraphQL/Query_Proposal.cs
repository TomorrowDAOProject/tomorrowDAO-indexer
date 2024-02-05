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

    [Name("getVoteInfoMemory")]
    public static async Task<VoteInfoDto> GetVoteInfoMemoryAsync(
        [FromServices] IAElfIndexerClientEntityRepository<VoteIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetVoteInfoInput input)
    {
        var voteInfo = await repository.GetFromBlockStateSetAsync(input.VotingItemId, input.ChainId);
        return voteInfo == null ? null : objectMapper.Map<VoteIndex, VoteInfoDto>(voteInfo);
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
}