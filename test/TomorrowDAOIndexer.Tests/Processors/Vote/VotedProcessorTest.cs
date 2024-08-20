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

        var voteId = HashHelper.ComputeFrom(Id3).ToHex();
        var voteRecordIndex = await GetIndexById<VoteRecordIndex>(voteId);
        voteRecordIndex.ShouldNotBeNull();
        voteRecordIndex.TransactionId.ShouldBe("4e07408562bedb8b60ce05c1decfe3ad16b72230967de01f640b7e4729b49fce");
        voteRecordIndex.Memo.ShouldBe("Memo");
        
        var latestParticipatedIndex = await GetIndexById<LatestParticipatedIndex>(IdGenerateHelper.GetId(ChainId, daoId, User));
        latestParticipatedIndex.ShouldNotBeNull();
        latestParticipatedIndex.Address.ShouldBe(User);
        latestParticipatedIndex.DAOId.ShouldBe(daoId);
        latestParticipatedIndex.ParticipatedType.ShouldBe(ParticipatedType.Voted);

        var daoVoterRecordIndex = await GetIndexById<DaoVoterRecordIndex>(IdGenerateHelper.GetId(ChainId, daoId, User));
        daoVoterRecordIndex.ShouldNotBeNull();
        daoVoterRecordIndex.Count.ShouldBe(1);
        daoVoterRecordIndex.Amount.ShouldBe(100);
    }
}