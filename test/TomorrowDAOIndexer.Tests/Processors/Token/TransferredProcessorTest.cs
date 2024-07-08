using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
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
}