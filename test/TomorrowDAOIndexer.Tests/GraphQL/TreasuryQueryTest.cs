using AElf;
using Shouldly;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetTreasuryFundAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        // await MockEventProcess(TokenTransferred(), TransferredProcessor);
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(TokenTransferred()));

        var list1 = await Query.GetTreasuryFundAsync(TreasuryFundSumRepository, ObjectMapper,
            new GetTreasuryFundInput { ChainId = ChainId });
        var list2 = await Query.GetTreasuryFundByFundListAsync(TreasuryFundRepository, new GetTreasuryFundInput { ChainId = ChainId });
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
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(TokenTransferred()));
        
        var result = await Query.GetAllTreasuryFundListAsync(TreasuryFundRepository, ObjectMapper, new GetAllTreasuryFundListInput
        {
            ChainId = ChainId, DaoId = HashHelper.ComputeFrom(Id1).ToHex()
        });
        result.ShouldNotBeNull();
        result.Item1.ShouldBe(1);
        var treasuryFundDtoList = result.Item2;
        var treasuryFundDto = treasuryFundDtoList[0];
        treasuryFundDto.DaoId.ShouldBe(DAOId);
        treasuryFundDto.Symbol.ShouldBe(Elf);
        treasuryFundDto.AvailableFunds.ShouldBe(100000000);
        treasuryFundDto.LockedFunds.ShouldBe(0);
    }

    [Fact]
    public async Task GetTreasuryFundListAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(TokenTransferred()));
        await MockEventProcess(TreasuryTransferred(), TreasuryTransferredProcessor);
        
        var result = await Query.GetTreasuryFundListAsync(TreasuryFundRepository, ObjectMapper, new GetTreasuryFundListInput
        {
            SkipCount = 0,
            ChainId = ChainId,
            StartBlockHeight = BlockHeight,
            EndBlockHeight = BlockHeight + 1,
            DaoId = HashHelper.ComputeFrom(Id1).ToHex(),
            // TreasuryAddress = null,
            MaxResultCount = 10
        });
        result.ShouldNotBeNull();
        result.Item1.ShouldBe(1);
        var treasuryFunds = result.Item2;
        var treasuryFundDto = treasuryFunds[0];
        treasuryFundDto.DaoId.ShouldBe(DAOId);
        treasuryFundDto.Symbol.ShouldBe(Elf);
        treasuryFundDto.AvailableFunds.ShouldBe(99999999);
        treasuryFundDto.LockedFunds.ShouldBe(0);
    }

    [Fact]
    public async Task GetTreasuryRecordListAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(TokenTransferred()));

        var daoId = HashHelper.ComputeFrom(Id1).ToHex();
        var treasuryRecords = await Query.GetTreasuryRecordListAsync(TreasuryRecordRepository, ObjectMapper, new GetTreasuryRecordListInput
        {
            SkipCount = 0,
            ChainId = ChainId,
            StartBlockHeight = 0,
            EndBlockHeight = 0,
            DaoId = daoId,
            TreasuryAddress = TreasuryAccountAddress,
            FromAddress = ExecuteAddress,
            Symbols = new List<string>(){Elf},
            MaxResultCount = 10
        });
        treasuryRecords.ShouldNotBeNull();
        treasuryRecords.Count.ShouldBe(1);
        var treasuryRecordDto = treasuryRecords[0];
        treasuryRecordDto.DaoId.ShouldBe(DAOId);
        treasuryRecordDto.Amount.ShouldBe(100000000);
        treasuryRecordDto.Symbol.ShouldBe(Elf);
        treasuryRecordDto.Executor.ShouldBe(ExecuteAddress);
        treasuryRecordDto.FromAddress.ShouldBe(ExecuteAddress);
        treasuryRecordDto.ToAddress.ShouldBe(TreasuryAccountAddress);
        treasuryRecordDto.TreasuryRecordType.ShouldBe((int)TreasuryRecordType.Deposit);
    }
}