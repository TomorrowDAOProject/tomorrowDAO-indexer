using AElf;
using AElf.Types;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Vote;

public class CommittedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(true), VotingItemRegisteredProcessor);
        await MockEventProcess(Committed(), CommittedProcessor);

        var id = IdGenerateHelper.GetId(TransactionId);
        var commitmentIndex = await GetIndexById<CommitmentIndex>(id);
        commitmentIndex.ShouldNotBeNull();
        commitmentIndex.TransactionId.ShouldBe(TransactionId);
        commitmentIndex.Commitment.ShouldBe(Commitment);
        commitmentIndex.LeafIndex.ShouldBe(1);

        var daoId = HashHelper.ComputeFrom(Id1).ToHex();

        var latestParticipatedIndex =
            await GetIndexById<LatestParticipatedIndex>(IdGenerateHelper.GetId(ChainId, daoId,
                "2EM5uV6bSJh6xJfZTUa1pZpYsYcCUAdPvZvFUJzMDJEx3rbioz"));
        latestParticipatedIndex.ShouldNotBeNull();
        latestParticipatedIndex.Address.ShouldBe("2EM5uV6bSJh6xJfZTUa1pZpYsYcCUAdPvZvFUJzMDJEx3rbioz");
        latestParticipatedIndex.DAOId.ShouldBe(daoId);
        latestParticipatedIndex.ParticipatedType.ShouldBe(ParticipatedType.Committed);
    }
}