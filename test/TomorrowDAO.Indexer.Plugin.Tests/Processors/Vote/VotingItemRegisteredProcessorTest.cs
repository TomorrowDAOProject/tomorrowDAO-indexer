using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Vote;

[CollectionDefinition(ClusterCollection.Name)]
public class VotingItemRegisteredProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteSchemeCreatedProcessor);
        
        var voteSchemeIndex = await VoteSchemeIndexRepository.GetFromBlockStateSetAsync(VoteSchemeId, ChainAelf);
        voteSchemeIndex.ShouldNotBeNull();
        voteSchemeIndex.VoteSchemeId.ShouldBe(VoteSchemeId);
        voteSchemeIndex.Id.ShouldBe(VoteSchemeId);
        voteSchemeIndex.VoteMechanism.ShouldBe(VoteMechanism.UNIQUE_VOTE);
        
        voteSchemeIndex = await VoteSchemeIndexRepository.GetFromBlockStateSetAsync(VoteSchemeId, ChainAelf);
        voteSchemeIndex.ShouldNotBeNull();
    }
}