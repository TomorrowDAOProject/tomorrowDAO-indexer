using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Token;

public class TransferredProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        //await MockEventProcess(TokenTransferred(), TransferredProcessor);

        var logEvent = TokenTransferred();
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var treasuryFundId = IdGenerateHelper.GetId(ChainId, DAOId, Elf);
        var treasuryFundIndex = await GetIndexById<TreasuryFundIndex>(treasuryFundId);
        treasuryFundIndex.ShouldNotBeNull();
        treasuryFundIndex.AvailableFunds.ShouldBe(100000000);
        treasuryFundIndex.BlockHeight.ShouldBe(100);
        var treasuryRecordId = IdGenerateHelper.GetId(ChainId,
            TransactionId, ExecuteAddress,
            TreasuryRecordType.Deposit);
        var treasuryRecordIndex = await GetIndexById<TreasuryRecordIndex>(treasuryRecordId);
        treasuryRecordIndex.ShouldNotBeNull();
        treasuryRecordIndex.Id.ShouldBe(treasuryRecordId);
        treasuryRecordIndex.DaoId.ShouldBe(DAOId);
        treasuryRecordIndex.Amount.ShouldBe(100000000L);
        treasuryRecordIndex.Symbol.ShouldBe(Elf);
        treasuryRecordIndex.Executor.ShouldBe(ExecuteAddress);
        treasuryRecordIndex.FromAddress.ShouldBe(ExecuteAddress);
        treasuryRecordIndex.ToAddress.ShouldBe(TreasuryAccountAddress);
        treasuryRecordIndex.TreasuryRecordType.ShouldBe(TreasuryRecordType.Deposit);
        treasuryRecordIndex.BlockHeight.ShouldBe(100);
    }

    [Fact]
    public async Task HandleEventAsync_TreasuryFundNotExist_Test()
    {
        var LogEvent = TokenTransferred();
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(LogEvent));
    }

    [Fact]
    public async Task HandleEventAsync_NotVotigramNFT_Test()
    {
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(tDVW, TokenTransferred()));
        var fromId = IdGenerateHelper.GetId(ExecuteAddress, tDVW, Elf);
        var toId = IdGenerateHelper.GetId(TreasuryAccountAddress, tDVW, Elf);
        var fromUserBalance = await GetIndexById<UserBalanceIndex>(fromId);
        var toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        fromUserBalance.ShouldBeNull();
        toUserBalance.ShouldBeNull();
        
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(tDVV, TokenTransferred()));
        fromId = IdGenerateHelper.GetId(ExecuteAddress, tDVV, Elf);
        toId = IdGenerateHelper.GetId(TreasuryAccountAddress, tDVV, Elf);
        fromUserBalance = await GetIndexById<UserBalanceIndex>(fromId);
        toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        fromUserBalance.ShouldBeNull();
        toUserBalance.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_VotigramNFT_Test()
    {
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVW, Issued(TOMORROWPASSTEST)));
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(tDVW, TokenTransferred(TOMORROWPASSTEST)));
        var fromId = IdGenerateHelper.GetId(ExecuteAddress, tDVW, TOMORROWPASSTEST);
        var toId = IdGenerateHelper.GetId(TreasuryAccountAddress, tDVW, TOMORROWPASSTEST);
        var fromUserBalance = await GetIndexById<UserBalanceIndex>(fromId);
        var toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        fromUserBalance.ShouldNotBeNull();
        fromUserBalance.Amount.ShouldBe(100000000);
        toUserBalance.ShouldNotBeNull();
        toUserBalance.Amount.ShouldBe(100000000);
        
        await IssuedProcessor.ProcessAsync(GenerateLogEventContext(tDVV, Issued(TOMORROWPASS)));
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(tDVV, TokenTransferred(TOMORROWPASS)));
        fromId = IdGenerateHelper.GetId(ExecuteAddress, tDVV, TOMORROWPASS);
        toId = IdGenerateHelper.GetId(TreasuryAccountAddress, tDVV, TOMORROWPASS);
        fromUserBalance = await GetIndexById<UserBalanceIndex>(fromId);
        toUserBalance = await GetIndexById<UserBalanceIndex>(toId);
        fromUserBalance.ShouldNotBeNull();
        fromUserBalance.Amount.ShouldBe(100000000);
        toUserBalance.ShouldNotBeNull();
        toUserBalance.Amount.ShouldBe(100000000);
    }
}