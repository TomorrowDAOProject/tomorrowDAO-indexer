using AElf;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VotingItemRegisteredProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        
        var voteItemIndex = await GetIndexById<VoteItemIndex>(HashHelper.ComputeFrom(Id2).ToHex());
        voteItemIndex.ShouldNotBeNull();
        voteItemIndex.VoteSchemeId.ShouldBe(HashHelper.ComputeFrom(Id4).ToHex());
        voteItemIndex.Id.ShouldBe(HashHelper.ComputeFrom(Id2).ToHex());
        voteItemIndex.VotingItemId.ShouldBe(HashHelper.ComputeFrom(Id2).ToHex());
    }
}