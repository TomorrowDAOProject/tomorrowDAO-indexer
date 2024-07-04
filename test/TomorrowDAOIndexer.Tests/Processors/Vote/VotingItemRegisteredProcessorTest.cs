using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VotingItemRegisteredProcessorTest : TomorrowDAOIndexerTestBase
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
        
        voteSchemeIndex = await GetIndexById<VoteSchemeIndex>(VoteSchemeId);
        voteSchemeIndex.ShouldNotBeNull();
    }
}