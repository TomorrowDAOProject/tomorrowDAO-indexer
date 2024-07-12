using Shouldly;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetSyncProposalInfosAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);

        var result = await Query.GetSyncProposalInfosAsync(ProposalIndexRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1,
            ChainId = ChainId, SkipCount = 0, MaxResultCount = 10
        });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        var proposal = result[0];
        proposal.DAOId.ShouldBe(DAOId);
    }

    [Fact]
    public async Task GetProposalCountAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        await MockEventProcess(ProposalCreated_Veto(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        var result = await Query.GetProposalCountAsync(ProposalIndexRepository, new GetProposalCountInput
        {
            ChainId = ChainId,
            DaoId = DAOId,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        result.Count.ShouldBe(2);
    }
}