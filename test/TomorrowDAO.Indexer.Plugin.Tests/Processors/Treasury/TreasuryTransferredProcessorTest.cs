using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Treasury;

[CollectionDefinition(ClusterCollection.Name)]
public class TreasuryTransferredProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(TokenTransferred(), TransferredProcessor);
        await MockEventProcess(TreasuryTransferred(), TreasuryTransferredProcessor);
        
        var treasuryFundId = IdGenerateHelper.GetId(ChainAelf, DAOId, Elf);
        var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(treasuryFundId, ChainAelf);
        treasuryFundIndex.ShouldNotBeNull();
        treasuryFundIndex.AvailableFunds.ShouldBe(99999999L);
        var treasuryRecordId = IdGenerateHelper.GetId(ChainAelf, "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2", ExecuteAddress, TreasuryRecordType.Transfer);
        var treasuryRecordIndex = await TreasuryRecordRepository.GetFromBlockStateSetAsync(treasuryRecordId, ChainAelf);
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
    }
    
    [Fact]
    public async Task HandleEventAsync_TreasuryFundNotExist_Test()
    {
        await MockEventProcess(TreasuryTransferred(), TreasuryTransferredProcessor);
    }
}