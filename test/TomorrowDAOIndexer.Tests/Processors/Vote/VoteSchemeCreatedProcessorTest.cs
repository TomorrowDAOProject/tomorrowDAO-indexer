using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VoteSchemeCreatedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteSchemeCreatedProcessor);
        
        var voteSchemeIndex = await GetIndexById<VoteSchemeIndex>(VoteSchemeId);
        voteSchemeIndex.ShouldNotBeNull();
        voteSchemeIndex.VoteSchemeId.ShouldBe(VoteSchemeId);
        voteSchemeIndex.Id.ShouldBe(VoteSchemeId);
        voteSchemeIndex.VoteMechanism.ShouldBe(VoteMechanism.UNIQUE_VOTE);
        voteSchemeIndex.VoteStrategy.ShouldBe(VoteStrategy.PROPOSAL_DISTINCT);
        voteSchemeIndex.WithoutLockToken.ShouldBe(true);
        
        voteSchemeIndex = await GetIndexById<VoteSchemeIndex>(VoteSchemeId);
        voteSchemeIndex.ShouldNotBeNull();
    }
}