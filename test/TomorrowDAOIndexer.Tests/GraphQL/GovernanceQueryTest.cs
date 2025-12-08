using AElf;
using AElf.Types;
using Shouldly;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetGovernanceSchemeAsync_Test()
    {
        await MockEventProcess(MockGovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);

        var result = await Query.GetGovernanceSchemeAsync(GovernanceSchemeRepository, ObjectMapper,
            new GovernanceSchemeIndexInput
            {
                ChainId = ChainId,
                DAOId = HashHelper.ComputeFrom("DaoId").ToString(),
            });
        result.ShouldNotBeNull();
    }

    private static GovernanceSchemeAdded MockGovernanceSchemeAdded()
    {
        return new GovernanceSchemeAdded
        {
            DaoId = HashHelper.ComputeFrom("DaoId"),
            SchemeId = HashHelper.ComputeFrom("06c84e65f48d95959cb580bfe13c45a3f5eec2ecb7851dc44e2f0b4362adafbc"),
            SchemeAddress = Address.FromBase58("8XepdGhyo27gUQNVzqq7GVvdEvMDXcxqPuQpXvBkooxzDb34S"),
            GovernanceMechanism = GovernanceMechanism.HighCouncil,
            SchemeThreshold = new GovernanceSchemeThreshold
            {
                MinimalRequiredThreshold = 1,
                MinimalVoteThreshold = 2,
                MinimalApproveThreshold = 1,
                MaximalRejectionThreshold = 1,
                MaximalAbstentionThreshold = 1
            },
            GovernanceToken = "ELF"
        };
    }
}