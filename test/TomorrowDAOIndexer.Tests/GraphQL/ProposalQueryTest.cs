using Shouldly;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetElectionListAsync_Test()
    {
        await MockEventProcess(VoteSchemeCreated_UniqueVote(), VoteSchemeCreatedProcessor);
        await MockEventProcess(VoteSchemeCreated_TokenBallot(), VoteSchemeCreatedProcessor);
        
        // var voteSchemes = await Query.GetVoteSchemeInfoAsync(VoteSchemeIndexRepository, ObjectMapper, new GetVoteSchemeInput
        // {
        //     ChainId = ChainAelf,
        //     Types = new List<int>{1}
        // });
        // voteSchemes.ShouldNotBeNull();
        // voteSchemes.Count.ShouldBe(1);
        // var voteScheme = voteSchemes[0];
        // voteScheme.ChainId.ShouldBe(ChainAelf);
        // voteScheme.VoteMechanism.ShouldBe(VoteMechanism.UniqueVote);
        // voteScheme.VoteSchemeId.ShouldBe(VoteSchemeId);
        // voteScheme.IsQuadratic.ShouldBe(true);
        // voteScheme.IsLockToken.ShouldBe(true);
    }
    
    [Fact]
    public async Task GetProposalCountAsyncTest()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        await MockEventProcess(ProposalCreated_Veto(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        var count = await Query.GetProposalCountAsync(ProposalIndexRepository, ObjectMapper, new GetProposalCountInput
        {
            ChainId = ChainId,
            //DaoId = null,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        count.ShouldBe(2);
    }
}