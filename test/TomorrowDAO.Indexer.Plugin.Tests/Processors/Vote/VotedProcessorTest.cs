using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Vote;

public class VotedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    private const string TransactionId = "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2";
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        
        var daoId = HashHelper.ComputeFrom(Id1).ToHex();
        var daoIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(daoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.VoteAmount.ShouldBe(100);

        var latestParticipatedIndex = await LatestParticipatedIndexRepository.GetFromBlockStateSetAsync(IdGenerateHelper.GetId(ChainAelf, User), ChainAelf);
        latestParticipatedIndex.ShouldNotBeNull();
        latestParticipatedIndex.Address.ShouldBe(User);
        latestParticipatedIndex.DAOId.ShouldBe(daoId);
        latestParticipatedIndex.ParticipatedType.ShouldBe(ParticipatedType.Voted);
    }
}