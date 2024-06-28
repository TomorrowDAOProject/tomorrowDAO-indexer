using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class TreasuryQueryTest : QueryTestBase
{
    [Fact]
    public async Task GetTreasuryFundAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(TokenTransferred(), TransferredProcessor);

        var list1 = await Query.GetTreasuryFundAsync(TreasuryFundSumRepository, ObjectMapper,
            new GetTreasuryFundInput { ChainId = ChainAelf });
        var list2 = await Query.GetTreasuryFundByFundListAsync(TreasuryFundRepository, new GetTreasuryFundInput { ChainId = ChainAelf });
        CheckTreasuryFund(list1);
        CheckTreasuryFund(list2);
    }

    private void CheckTreasuryFund(List<GetDAOAmountRecordDto> list)
    {
        list.ShouldNotBeNull();
        list.Count.ShouldBe(1);
        var fundSum = list[0];
        fundSum.ShouldNotBeNull();
        fundSum.GovernanceToken.ShouldBe(Elf);
        fundSum.Amount.ShouldBe(100000000);
    }

    [Fact]
    public async Task GetAllTreasuryFundListAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(TokenTransferred(), TransferredProcessor);
        
        var(count, treasuryFunds) = await Query.GetAllTreasuryFundListAsync(TreasuryFundRepository, ObjectMapper, new GetAllTreasuryFundListInput
        {
            ChainId = ChainAelf, DaoId = HashHelper.ComputeFrom(Id1).ToHex()
        });
        treasuryFunds.ShouldNotBeNull();
        treasuryFunds.Count.ShouldBe(1);
        var treasuryFundDto = treasuryFunds[0];
        treasuryFundDto.DaoId.ShouldBe(DAOId);
        treasuryFundDto.Symbol.ShouldBe(Elf);
        treasuryFundDto.AvailableFunds.ShouldBe(100000000);
        treasuryFundDto.LockedFunds.ShouldBe(0);
        count.ShouldBe(1);
    }

    [Fact]
    public async Task GetTreasuryFundListAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(TokenTransferred(), TransferredProcessor);
        await MockEventProcess(TreasuryTransferred(), TreasuryTransferredProcessor);
        
        var(count, treasuryFunds) = await Query.GetTreasuryFundListAsync(TreasuryFundRepository, ObjectMapper, new GetTreasuryFundListInput
        {
            SkipCount = 0,
            ChainId = ChainAelf,
            StartBlockHeight = BlockHeight,
            EndBlockHeight = BlockHeight + 1,
            DaoId = HashHelper.ComputeFrom(Id1).ToHex(),
            TreasuryAddress = null,
            MaxResultCount = 10
        });
        treasuryFunds.ShouldNotBeNull();
        treasuryFunds.Count.ShouldBe(1);
        var treasuryFundDto = treasuryFunds[0];
        treasuryFundDto.DaoId.ShouldBe(DAOId);
        treasuryFundDto.Symbol.ShouldBe(Elf);
        treasuryFundDto.AvailableFunds.ShouldBe(99999999);
        treasuryFundDto.LockedFunds.ShouldBe(0);
        count.ShouldBe(1);
    }

    [Fact]
    public async Task GetTreasuryRecordListAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(TokenTransferred(), TransferredProcessor);
        
        // var treasuryRecords = await Query.GetTreasuryRecordListAsync(TreasuryRecordRepository, ObjectMapper, new GetChainBlockHeightInput
        // {
        //     ChainId = ChainAelf,
        //     StartBlockHeight = BlockHeight,
        //     EndBlockHeight = BlockHeight + 1,
        //     MaxResultCount = 10
        // });
        // treasuryRecords.ShouldNotBeNull();
        // treasuryRecords.Count.ShouldBe(1);
        // var treasuryRecordDto = treasuryRecords[0];
        // treasuryRecordDto.DAOId.ShouldBe(DAOId);
        // treasuryRecordDto.Amount.ShouldBe(1L);
        // treasuryRecordDto.Symbol.ShouldBe(Elf);
        // treasuryRecordDto.Executor.ShouldBe(DAOCreator);
        // treasuryRecordDto.FromAddress.ShouldBe(DAOCreator);
        // treasuryRecordDto.ToAddress.ShouldBe(TreasuryAccountAddress);
        // treasuryRecordDto.TreasuryRecordType.ShouldBe(TreasuryRecordType.Donate);
    }
}