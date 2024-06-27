using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using Moq;
using Shouldly;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

[CollectionDefinition(ClusterCollection.Name)]
public class GovernanceQueryTest : QueryTestBase
{
    [Fact]
    public async Task GetGovernanceSchemeAsync_Test()
    {
        await MockEventProcess(MockGovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);

        var result = await Query.GetGovernanceSchemeAsync(GovernanceSchemeRepository, ObjectMapper,
            new GovernanceSchemeIndexInput
            {
                ChainId = ChainAelf,
                DAOId = HashHelper.ComputeFrom("DaoId").ToString(),
            });
        result.ShouldNotBeNull();
    }

    private static LogEvent MockGovernanceSchemeAdded()
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
        }.ToLogEvent();
    }
}