using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Vote;

public class VoteCreatedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteCreatedProcessor);
        
        var voteSchemeIndex = await VoteSchemeIndexRepository.GetFromBlockStateSetAsync(VoteSchemeId, ChainAelf);
        voteSchemeIndex.ShouldNotBeNull();
        voteSchemeIndex.VoteSchemeId.ShouldBe(VoteSchemeId);
        voteSchemeIndex.Id.ShouldBe(VoteSchemeId);
        voteSchemeIndex.VoteMechanism.ShouldBe(VoteMechanism.UniqueVote);
        voteSchemeIndex.IsQuadratic.ShouldBe(true);
        voteSchemeIndex.IsLockToken.ShouldBe(true);
        
        voteSchemeIndex = await VoteSchemeIndexRepository.GetFromBlockStateSetAsync(VoteSchemeId, ChainAelf);
        voteSchemeIndex.ShouldNotBeNull();
    }
}