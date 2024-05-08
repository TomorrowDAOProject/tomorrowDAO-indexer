using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class ProposalQueryTest : QueryTestBase
{
    [Fact]
    public async Task GetElectionListAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteSchemeCreatedProcessor);
        await MockEventProcess(VoteSchemeCreated_TokenBallot(), VoteSchemeCreatedProcessor);
        
        // var voteSchemes = await Query.GetVoteSchemeInfoAsync(VoteSchemeIndexRepository, ObjectMapper, new GetVoteSchemeInput
        // {
        //     ChainId = ChainAelf,
        //     Types = new List<int>{1}
        // });
        // voteSchemes.ShouldNotBeNull();
        // voteSchemes.Count.ShouldBe(1);
        // var voteScheme = voteSchemes[0];
        // voteScheme.ChainId.ShouldBe(ChainAelf);
        // voteScheme.VoteMechanism.ShouldBe(VoteMechanism.UniqueVote);
        // voteScheme.VoteSchemeId.ShouldBe(VoteSchemeId);
        // voteScheme.IsQuadratic.ShouldBe(true);
        // voteScheme.IsLockToken.ShouldBe(true);
    }
}