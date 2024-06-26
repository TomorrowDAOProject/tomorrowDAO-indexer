using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Vote;

[CollectionDefinition(ClusterCollection.Name)]
public class VoteWithdrawnProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    private const string TransactionId = "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2";
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var id = IdGenerateHelper.GetId(ChainAelf, HashHelper.ComputeFrom(Id1).ToHex(), TransactionId);
        var voteSchemeIndex = await VoteWithdrawnRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        voteSchemeIndex.ShouldNotBeNull();
        voteSchemeIndex.WithdrawAmount.ShouldBe(10);
    }
    
    [Fact]
    public async Task HandleEventAsync_UpdateDaoAmount()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var daoId = HashHelper.ComputeFrom(Id1).ToHex();
        var daoIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(daoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.VoteAmount.ShouldBe(100);
        daoIndex.WithdrawAmount.ShouldBe(10);
    }
}