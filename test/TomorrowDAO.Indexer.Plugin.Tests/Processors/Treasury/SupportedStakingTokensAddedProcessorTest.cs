using Shouldly;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Treasury;

public class SupportedStakingTokensAddedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(SupportedStakingTokensAdded(), SupportedStakingTokensAddedProcessor);
        
        var treasuryFundId = IdGenerateHelper.GetId(ChainAelf, DAOId, Elf);
        var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(treasuryFundId, ChainAelf);
        treasuryFundIndex.ShouldNotBeNull();
        treasuryFundIndex.Id.ShouldBe(treasuryFundId);
        treasuryFundIndex.DAOId.ShouldBe(DAOId);
        treasuryFundIndex.IsRemoved.ShouldBe(false);
        treasuryFundIndex.Symbol.ShouldBe(Elf);
        treasuryFundIndex.AvailableFunds.ShouldBe(0);
        treasuryFundIndex.LockedFunds.ShouldBe(0);
    }
}