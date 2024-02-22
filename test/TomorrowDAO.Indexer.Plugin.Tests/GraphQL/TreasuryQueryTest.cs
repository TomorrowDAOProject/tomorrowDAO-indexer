using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class TreasuryQueryTest : QueryTestBase
{
    [Fact]
    public async Task GetTreasuryFundListAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        
        var treasuryFunds = await Query.GetTreasuryFundListAsync(TreasuryFundRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            ChainId = ChainAelf,
            StartBlockHeight = BlockHeight,
            EndBlockHeight = BlockHeight + 1,
            MaxResultCount = 10
        });
        treasuryFunds.ShouldNotBeNull();
        treasuryFunds.Count.ShouldBe(1);
        var treasuryFundDto = treasuryFunds[0];
        treasuryFundDto.DAOId.ShouldBe(DAOId);
        treasuryFundDto.IsRemoved.ShouldBe(false);
        treasuryFundDto.Symbol.ShouldBe(Elf);
        treasuryFundDto.AvailableFunds.ShouldBe(0);
        treasuryFundDto.LockedFunds.ShouldBe(0);
    }

    [Fact]
    public async Task GetTreasuryRecordListAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(DonationReceived(), DonationReceivedProcessor);
        
        var treasuryRecords = await Query.GetTreasuryRecordListAsync(TreasuryRecordRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            ChainId = ChainAelf,
            StartBlockHeight = BlockHeight,
            EndBlockHeight = BlockHeight + 1,
            MaxResultCount = 10
        });
        treasuryRecords.ShouldNotBeNull();
        treasuryRecords.Count.ShouldBe(1);
        var treasuryRecordDto = treasuryRecords[0];
        treasuryRecordDto.DAOId.ShouldBe(DAOId);
        treasuryRecordDto.Amount.ShouldBe(1L);
        treasuryRecordDto.Symbol.ShouldBe(Elf);
        treasuryRecordDto.Executor.ShouldBe(DAOCreator);
        treasuryRecordDto.FromAddress.ShouldBe(DAOCreator);
        treasuryRecordDto.ToAddress.ShouldBe(TreasuryAccountAddress);
        treasuryRecordDto.TreasuryRecordType.ShouldBe(TreasuryRecordType.Donate);
    }
}