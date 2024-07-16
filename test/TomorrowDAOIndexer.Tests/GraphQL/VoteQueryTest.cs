using AElf;
using AElf.Types;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetVoteRecordListAsync_Test()
    {
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        
        var result = await Query.GetVoteRecordListAsync(VoteRecordIndexRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            ChainId = ChainId, SkipCount = 0, MaxResultCount = 10, StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1
        });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetVoteWithdrawnListAsync_Test()
    {
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var result = await Query.GetVoteWithdrawnListAsync(VoteWithdrawnRepository, ObjectMapper, new GetChainBlockHeightInput
            {
                ChainId = ChainId, SkipCount = 0, MaxResultCount = 10, StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1
            });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetVoteSchemesAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteSchemeCreatedProcessor);
        await MockEventProcess(VoteSchemeCreated_TokenBallot(), VoteSchemeCreatedProcessor);
        
        var voteSchemes = await Query.GetVoteSchemesAsync(VoteSchemeIndexRepository, ObjectMapper, new GetVoteSchemeInput
        {
            ChainId = ChainId
        });
        voteSchemes.ShouldNotBeNull();
        voteSchemes.Count.ShouldBe(2);
        var voteScheme = voteSchemes[0];
        voteScheme.ChainId.ShouldBe(ChainId);
        voteScheme.VoteMechanism.ShouldBe(0);
        voteScheme.VoteSchemeId.ShouldBe(VoteSchemeId);
        voteScheme.IsQuadratic.ShouldBe(false);
        voteScheme.IsLockToken.ShouldBe(false);
    }

    [Fact]
    public async Task GetVoteItemIndexAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteSchemeCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);

        var result = await Query.GetVoteItemIndexAsync(VoteItemIndexRepository, ObjectMapper, new GetVoteInfoInput
        {
            ChainId = ChainId, VotingItemIds = new List<string>{HashHelper.ComputeFrom(Id2).ToHex()}
        });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        var voteItemIndex = result[0];
        voteItemIndex.VotingItemId.ShouldBe(HashHelper.ComputeFrom(Id2).ToHex());
    }

    [Fact]
    public async Task GetVoteSchemeInfoAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteSchemeCreatedProcessor);
        await MockEventProcess(VoteSchemeCreated_TokenBallot(), VoteSchemeCreatedProcessor);
        
        var voteSchemes = await Query.GetVoteSchemeInfoAsync(VoteSchemeIndexRepository, ObjectMapper, new GetVoteSchemeInput
        {
            ChainId = ChainId
        });
        voteSchemes.ShouldNotBeNull();
        voteSchemes.Count.ShouldBe(2);
        var voteScheme = voteSchemes[0];
        voteScheme.ChainId.ShouldBe(ChainId);
        voteScheme.VoteMechanism.ShouldBe(VoteMechanism.UNIQUE_VOTE);
        voteScheme.VoteSchemeId.ShouldBe(VoteSchemeId);
        voteScheme.IsQuadratic.ShouldBe(false);
        voteScheme.IsLockToken.ShouldBe(false);
    }
    
    [Fact]
    public async Task GetVoterWithdrawnIndexAsync_Test()
    {
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var id = IdGenerateHelper.GetId(ChainId, DAOId, TransactionId);
        var voteWithdrawnIndex = await GetIndexById<VoteWithdrawnIndex>(id);
        voteWithdrawnIndex.ShouldNotBeNull();

        var result = await Query.GetVoterWithdrawnIndexAsync(VoteWithdrawnRepository, ObjectMapper,
            new VoteWithdrawnIndexInput
            {
                ChainId = ChainId,
                DaoId = DAOId,
                Voter = Address.FromBase58(User).ToBase58()
            });
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllVoteRecordAsync_Test()
    {
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        
        var result = await Query.GetAllVoteRecordAsync(VoteRecordIndexRepository, ObjectMapper, new GetAllVoteRecordInput()
        {
            ChainId = ChainId, DAOId = DAOId, Voter = User
        });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetVoteRecordCountAsync_Test()
    {
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);

        var result = await Query.GetVoteRecordCountAsync(VoteRecordIndexRepository, new GetVoteRecordCountInput
        {
            ChainId = ChainId,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetLimitVoteRecordAsync_Test()
    {
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);

        var result = await Query.GetLimitVoteRecordAsync(VoteRecordIndexRepository, ObjectMapper, new GetLimitVoteRecordInput
        {
            ChainId = ChainId, Voter = User, Sorting = string.Empty, VotingItemId = HashHelper.ComputeFrom(Id2).ToHex(),
            Limit = 0
        });
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetPageVoteRecordAsync_Test()
    {
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);

        var result = await Query.GetPageVoteRecordAsync(VoteRecordIndexRepository, ObjectMapper, new GetPageVoteRecordInput
        {
            ChainId = ChainId, DaoId = DAOId, Voter = User, MaxResultCount = 10, SkipCount = 0,
            VoteOption = "Approved"
        });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        var voteRecord = result[0];
        voteRecord.Voter.ShouldBe(User);
        
        result = await Query.GetPageVoteRecordAsync(VoteRecordIndexRepository, ObjectMapper, new GetPageVoteRecordInput
        {
            ChainId = ChainId, DaoId = DAOId, Voter = User, MaxResultCount = 10, SkipCount = 0,
            VoteOption = "Abstained"
        });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(0);
    }
}