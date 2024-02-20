using Shouldly;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Treasury;

public class SupportedStakingTokensRemovedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(SupportedStakingTokensRemoved(), SupportedStakingTokensRemovedProcessor);
        
        var treasuryFundId = IdGenerateHelper.GetId(ChainAelf, DAOId, Elf);
        var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(treasuryFundId, ChainAelf);
        treasuryFundIndex.ShouldNotBeNull();
        treasuryFundIndex.Id.ShouldBe(treasuryFundId);
        treasuryFundIndex.DAOId.ShouldBe(DAOId);
        treasuryFundIndex.IsRemoved.ShouldBe(true);
        treasuryFundIndex.Symbol.ShouldBe(Elf);
        treasuryFundIndex.AvailableFunds.ShouldBe(0);
        treasuryFundIndex.LockedFunds.ShouldBe(0);
    }
    
    [Fact]
    public async Task HandleEventAsync_TreasuryFundNotExist_Test()
    {
        await MockEventProcess(SupportedStakingTokensRemoved(), SupportedStakingTokensRemovedProcessor);
    }
}