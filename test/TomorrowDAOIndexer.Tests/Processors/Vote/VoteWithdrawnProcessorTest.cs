using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VoteWithdrawnProcessorTest : TomorrowDAOIndexerTestBase
{
    
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var id = IdGenerateHelper.GetId(ChainId, HashHelper.ComputeFrom(Id1).ToHex(), TransactionId);
        var voteWithdrawnIndex = await GetIndexById<VoteWithdrawnIndex>(id);
        voteWithdrawnIndex.ShouldNotBeNull();
        voteWithdrawnIndex.WithdrawAmount.ShouldBe(10);
    }
    
    [Fact]
    public async Task HandleEventAsync_UpdateDaoAmount()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var daoId = HashHelper.ComputeFrom(Id1).ToHex();
        var daoIndex = await GetIndexById<DAOIndex>(daoId);
        daoIndex.ShouldNotBeNull();
        daoIndex.VoteAmount.ShouldBe(100);
        daoIndex.WithdrawAmount.ShouldBe(10);
    }
}