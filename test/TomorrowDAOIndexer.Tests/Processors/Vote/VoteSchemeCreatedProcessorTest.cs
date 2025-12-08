using AElf;
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

    [Fact]
    public async Task HandleEventAsync_Test_DailyNVote()
    {
        await MockEventProcess(VoteSchemeCreated_DailyNVote(), VoteSchemeCreatedProcessor);
        
        var voteSchemeId = HashHelper.ComputeFrom(Id4).ToHex();
        var voteSchemeIndex = await GetIndexById<VoteSchemeIndex>(voteSchemeId);
        voteSchemeIndex.ShouldNotBeNull();
        voteSchemeIndex.VoteSchemeId.ShouldBe(voteSchemeId);
        voteSchemeIndex.Id.ShouldBe(voteSchemeId);
        voteSchemeIndex.VoteMechanism.ShouldBe(VoteMechanism.TOKEN_BALLOT);
        voteSchemeIndex.VoteStrategy.ShouldBe(VoteStrategy.DAILY_N_VOTES);
        voteSchemeIndex.WithoutLockToken.ShouldBe(true);
        voteSchemeIndex.VoteCount.ShouldBe(20);
    }
}