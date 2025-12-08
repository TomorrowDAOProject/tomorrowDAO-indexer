using Shouldly;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Token;

public class BurnedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_NotVotigramNFT_Test()
    {
        await BurnedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Burned()));
        var id = IdGenerateHelper.GetId(ExecuteAddress, tDVW, Elf);
        var userBalance = await GetIndexById<UserBalanceIndex>(id);
        userBalance.ShouldBeNull();
        
        await BurnedProcessor.ProcessAsync(GenerateLogEventContext(tDVV, Burned()));
        id = IdGenerateHelper.GetId(ExecuteAddress, tDVV, Elf);
        userBalance = await GetIndexById<UserBalanceIndex>(id);
        userBalance.ShouldBeNull();
    }

    [Fact]
    public async Task HandleEventAsync_VotigramNFT_Test()
    {
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Issued(TOMORROWPASSTEST)));
        await BurnedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Burned(TOMORROWPASSTEST)));
        var id = IdGenerateHelper.GetId(ExecuteAddress, tDVW, TOMORROWPASSTEST);
        var userBalance = await GetIndexById<UserBalanceIndex>(id);
        userBalance.ShouldNotBeNull();
        userBalance.Amount.ShouldBe(100000000);
        userBalance.BlockHeight.ShouldBe(BlockHeight);
        
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVV, Issued(TOMORROWPASS)));
        await BurnedProcessor.ProcessAsync(GenerateLogEventContext(tDVV, Burned(TOMORROWPASS)));
        id = IdGenerateHelper.GetId(ExecuteAddress, tDVV, TOMORROWPASS);
        userBalance = await GetIndexById<UserBalanceIndex>(id);
        userBalance.ShouldNotBeNull();
        userBalance.Amount.ShouldBe(100000000);
        userBalance.BlockHeight.ShouldBe(BlockHeight);
    }
}