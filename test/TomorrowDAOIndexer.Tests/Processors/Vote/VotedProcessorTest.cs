using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VotedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        
        var daoId = HashHelper.ComputeFrom(Id1).ToHex();
        var daoIndex = await GetIndexById<DAOIndex>(daoId);
        daoIndex.ShouldNotBeNull();
        daoIndex.VoteAmount.ShouldBe(100);

        var latestParticipatedIndex = await GetIndexById<LatestParticipatedIndex>(IdGenerateHelper.GetId(ChainId, User));
        latestParticipatedIndex.ShouldNotBeNull();
        latestParticipatedIndex.Address.ShouldBe(User);
        latestParticipatedIndex.DAOId.ShouldBe(daoId);
        latestParticipatedIndex.ParticipatedType.ShouldBe(ParticipatedType.Voted);
    }
}