using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Treasury;

public class TreasuryTokenUnLockedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(DonationReceived(), DonationReceivedProcessor);
        await MockEventProcess(TreasuryTokenLocked(), TreasuryTokenLockedProcessor);
        await MockEventProcess(TreasuryTokenUnlocked(), TreasuryTokenUnlockedProcessor);
        
        var treasuryFundId = IdGenerateHelper.GetId(ChainAelf, DAOId, Elf);
        var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(treasuryFundId, ChainAelf);
        treasuryFundIndex.ShouldNotBeNull();
        treasuryFundIndex.LockedFunds.ShouldBe(0L);
        treasuryFundIndex.AvailableFunds.ShouldBe(1L);
        var treasuryRecordId = IdGenerateHelper.GetId(ChainAelf, "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2", DAOCreator, TreasuryRecordType.UnLock);
        var treasuryRecordIndex = await TreasuryRecordRepository.GetFromBlockStateSetAsync(treasuryRecordId, ChainAelf);
        treasuryRecordIndex.ShouldNotBeNull();
        treasuryRecordIndex.Id.ShouldBe(treasuryRecordId);
        treasuryRecordIndex.DAOId.ShouldBe(DAOId);
        treasuryRecordIndex.Amount.ShouldBe(1L);
        treasuryRecordIndex.Symbol.ShouldBe(Elf);
        treasuryRecordIndex.Executor.ShouldBe(DAOCreator);
        treasuryRecordIndex.FromAddress.ShouldBe(string.Empty);
        treasuryRecordIndex.ToAddress.ShouldBe(string.Empty);
        treasuryRecordIndex.TreasuryRecordType.ShouldBe(TreasuryRecordType.UnLock);
    }
    
    [Fact]
    public async Task HandleEventAsync_TreasuryFundNotExist_Test()
    {
        await MockEventProcess(TreasuryTokenUnlocked(), TreasuryTokenUnlockedProcessor);
    }
}