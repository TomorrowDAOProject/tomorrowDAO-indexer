using AElf;
using Shouldly;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetCommitmentsAsync_Test()
    {
        await MockEventProcess(Committed(), CommittedProcessor);

        var result = await Query.GetCommitmentsAsync(CommitmentIndexRepository, ObjectMapper,
            new GetChainBlockHeightInput
            {
                ChainId = ChainId, SkipCount = 0, MaxResultCount = 10, StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1
            });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetLimitCommitmentsAsync_Test()
    {
        await MockEventProcess(Committed(), CommittedProcessor);

        {
            var result = await Query.GetLimitCommitmentsAsync(CommitmentIndexRepository, ObjectMapper,
                new GetLimitCommitmentInput()
                {
                    ChainId = ChainId, Voter = User, Sorting = string.Empty,
                    VotingItemId = HashHelper.ComputeFrom(Id2).ToHex(),
                    Limit = 0
                });
            result.ShouldBeEmpty();
        }


        {
            var result = await Query.GetLimitCommitmentsAsync(CommitmentIndexRepository, ObjectMapper,
                new GetLimitCommitmentInput()
                {
                    ChainId = ChainId, Voter = "2EM5uV6bSJh6xJfZTUa1pZpYsYcCUAdPvZvFUJzMDJEx3rbioz",
                    Sorting = string.Empty, VotingItemId = HashHelper.ComputeFrom(Id2).ToHex(),
                    Limit = 0
                });
            result.ShouldNotBeEmpty();
        }
    }
}