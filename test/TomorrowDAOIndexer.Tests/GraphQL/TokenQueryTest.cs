using Shouldly;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetSyncUserBalanceInfosAsync_Test()
    {
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Issued(TOMORROWPASSTEST)));
        await BurnedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Burned(TOMORROWPASSTEST)));
        
        var result = await Query.GetSyncUserBalanceInfosAsync(UserBalanceIndexRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1,
            ChainId = ChainId, SkipCount = 0, MaxResultCount = 10
        });
       result.Count.ShouldBe(1);
       var userBalanceDto = result[0];
       userBalanceDto.Amount.ShouldBe(100000000);
       userBalanceDto.BlockHeight.ShouldBe(BlockHeight);
       userBalanceDto.ChainId.ShouldBe(ChainId);
    }
}