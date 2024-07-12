using Shouldly;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetProposalCountAsyncTest()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        await MockEventProcess(ProposalCreated_Veto(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        var result = await Query.GetProposalCountAsync(ProposalIndexRepository, new GetProposalCountInput
        {
            ChainId = ChainId,
            //DaoId = null,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        result.Count.ShouldBe(2);
    }
}