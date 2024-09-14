using Shouldly;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Token;

public class IssuedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_NotVotigramNFT_Test()
    {
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Issued()));
        var toId = IdGenerateHelper.GetId(ExecuteAddress, tDVW, Elf);
        var toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        toUserBalance.ShouldBeNull();
        
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVV, Issued()));
        toId = IdGenerateHelper.GetId(ExecuteAddress, tDVV, TOMORROWPASS);
        toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        toUserBalance.ShouldBeNull();
    }

    [Fact]
    public async Task HandleEventAsync_VotigramNFT_Test()
    {
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Issued(TOMORROWPASSTEST)));
        var toId = IdGenerateHelper.GetId(ExecuteAddress, tDVW, TOMORROWPASSTEST);
        var toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        toUserBalance.ShouldNotBeNull();
        toUserBalance.Amount.ShouldBe(200000000);
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Issued(TOMORROWPASSTEST)));
        toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        toUserBalance.ShouldNotBeNull();
        toUserBalance.Amount.ShouldBe(400000000);
        
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVV, Issued(TOMORROWPASS)));
        toId = IdGenerateHelper.GetId(ExecuteAddress, tDVV, TOMORROWPASS);
        toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        toUserBalance.ShouldNotBeNull();
        toUserBalance.Amount.ShouldBe(200000000);
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVV, Issued(TOMORROWPASS)));
        toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        toUserBalance.ShouldNotBeNull();
        toUserBalance.Amount.ShouldBe(400000000);
    }
}