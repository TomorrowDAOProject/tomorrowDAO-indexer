using Shouldly;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Treasury;

public class TreasuryCreatedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.TreasuryAccountAddress.ShouldBe(TreasuryAccountAddress);
        var treasuryFundId = IdGenerateHelper.GetId(ChainAelf, DAOId, Elf);
        var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(treasuryFundId, ChainAelf);
        treasuryFundIndex.ShouldNotBeNull();
        treasuryFundIndex.Id.ShouldBe(treasuryFundId);
        treasuryFundIndex.DAOId.ShouldBe(DAOId);
        treasuryFundIndex.Symbol.ShouldBe(Elf);
        treasuryFundIndex.AvailableFunds.ShouldBe(0);
        treasuryFundIndex.LockedFunds.ShouldBe(0);
    }
    
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
    }
}