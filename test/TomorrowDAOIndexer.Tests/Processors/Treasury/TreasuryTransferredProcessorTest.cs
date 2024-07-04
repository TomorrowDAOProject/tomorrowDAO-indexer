using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Treasury;

public class TreasuryTransferredProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(TokenTransferred(), TransferredProcessor);
        await MockEventProcess(TreasuryTransferred(), TreasuryTransferredProcessor);

        var treasuryFundId = IdGenerateHelper.GetId(ChainId, DAOId, Elf);
        var treasuryFundIndex = await GetIndexById<TreasuryFundIndex>(treasuryFundId);
        treasuryFundIndex.ShouldNotBeNull();
        treasuryFundIndex.AvailableFunds.ShouldBe(99999999L);
        var treasuryRecordId = IdGenerateHelper.GetId(ChainId,
            "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2", ExecuteAddress,
            TreasuryRecordType.Transfer);
        var treasuryRecordIndex = await GetIndexById<TreasuryRecordIndex>(treasuryRecordId);
        treasuryRecordIndex.ShouldNotBeNull();
        treasuryRecordIndex.Id.ShouldBe(treasuryRecordId);
        treasuryRecordIndex.DaoId.ShouldBe(DAOId);
        treasuryRecordIndex.Amount.ShouldBe(1L);
        treasuryRecordIndex.Symbol.ShouldBe(Elf);
        treasuryRecordIndex.Executor.ShouldBe(ExecuteAddress);
        treasuryRecordIndex.FromAddress.ShouldBe(TreasuryAccountAddress);
        treasuryRecordIndex.TreasuryAddress.ShouldBe(TreasuryAccountAddress);
        treasuryRecordIndex.ToAddress.ShouldBe(DAOCreator);
        treasuryRecordIndex.TreasuryRecordType.ShouldBe(TreasuryRecordType.Transfer);

        var fundSumId = IdGenerateHelper.GetId(ChainId, Elf);
        var fundSumIndex = await GetIndexById<TreasuryFundSumIndex>(fundSumId);
        fundSumIndex.AvailableFunds.ShouldBe(99999999L);
    }

    [Fact]
    public async Task HandleEventAsync_TreasuryFundNotExist_Test()
    {
        await MockEventProcess(TreasuryTransferred(), TreasuryTransferredProcessor);
    }
}