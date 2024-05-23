using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;
using Xunit.Abstractions;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

public class ProposalProcessorTests : TomorrowDAOIndexerPluginTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ProposalProcessorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ProposalCreated_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        await MockEventProcess(ProposalCreated_Veto(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex = await ProposalIndexRepository.GetFromBlockStateSetAsync(proposalId, ChainAelf);
        proposalIndex.ShouldNotBeNull();
        proposalIndex.Id.ShouldBe(proposalId);
        proposalIndex.DAOId.ShouldBe(DAOId);
        proposalIndex.ProposalId.ShouldBe(proposalId);
        proposalIndex.ProposalTitle.ShouldBe(ProposalTitle);
        proposalIndex.ProposalDescription.ShouldBe(ProposalDescription);
        proposalIndex.ForumUrl.ShouldBe(ForumUrl);
        proposalIndex.ProposalType.ShouldBe(ProposalType.Advisory);
        proposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Empty);
        proposalIndex.ProposalStage.ShouldBe(ProposalStage.Active);
        proposalIndex.Proposer.ShouldBe(DAOCreator);
        proposalIndex.SchemeAddress.ShouldBe(SchemeAddress);
        proposalIndex.VoteSchemeId.ShouldBe(VoteSchemeId);
        proposalIndex.VetoProposalId.ShouldBe(VetoProposalId);
        proposalIndex.GovernanceMechanism.ShouldBe(GovernanceMechanism.Referendum);
        proposalIndex.MinimalRequiredThreshold.ShouldBe(10);
        proposalIndex.MinimalVoteThreshold.ShouldBe(12);
        proposalIndex.MinimalApproveThreshold.ShouldBe(50);
        proposalIndex.MaximalRejectionThreshold.ShouldBe(30);
        proposalIndex.MaximalAbstentionThreshold.ShouldBe(20);
        proposalIndex.ActiveTimePeriod.ShouldBe(1);
        proposalIndex.VetoActiveTimePeriod.ShouldBe(5);
        proposalIndex.PendingTimePeriod.ShouldBe(3);
        proposalIndex.ExecuteTimePeriod.ShouldBe(2);
        proposalIndex.VetoExecuteTimePeriod.ShouldBe(4);
        var transaction = proposalIndex.Transaction;
        transaction.ShouldNotBeNull();
        transaction.ContractMethodName.ShouldBe("ContractMethodName");
        transaction.ToAddress.ShouldBe(ExecuteAddress);
        
        var vetoProposalIndex = await ProposalIndexRepository.GetFromBlockStateSetAsync(VetoProposalId, ChainAelf);
        vetoProposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Challenged);
        vetoProposalIndex.ProposalStage.ShouldBe(ProposalStage.Pending);
    }
    
    [Fact]
    public async Task ProposalExecuted_Test()
    {
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalExecuted(), ProposalExecutedProcessor);
        
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex = await ProposalIndexRepository.GetFromBlockStateSetAsync(proposalId, ChainAelf);
        proposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Executed);
        proposalIndex.ProposalStage.ShouldBe(ProposalStage.Finished);
    }

    [Fact]
    public async Task ProposalVetoed_Test()
    {
        //ProposalId
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        //Id4
        await MockEventProcess(ProposalCreated_Veto(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalExecuted(Id4), ProposalExecutedProcessor);
        await MockEventProcess(ProposalVetoed(), ProposalVetoedProcessor);
        
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex = await ProposalIndexRepository.GetFromBlockStateSetAsync(proposalId, ChainAelf);
        proposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Vetoed);
        proposalIndex.ProposalStage.ShouldBe(ProposalStage.Finished);
        
        var vetoProposalIndex = await ProposalIndexRepository.GetFromBlockStateSetAsync(VetoProposalId, ChainAelf);
        vetoProposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Executed);
        vetoProposalIndex.ProposalStage.ShouldBe(ProposalStage.Finished);
    }

    [Fact]
    public async Task DAOProposalTimePeriodSet_Test()
    {
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.ActiveTimePeriod.ShouldBe(1);
        DAOIndex.VetoActiveTimePeriod.ShouldBe(5);
        DAOIndex.PendingTimePeriod.ShouldBe(3);
        DAOIndex.ExecuteTimePeriod.ShouldBe(2);
        DAOIndex.VetoExecuteTimePeriod.ShouldBe(4);
    }

    [Fact]
    public Task ExecuteTransactionParamTest()
    {
        var param = "506172616d73";
        var byteString = ByteStringHelper.FromHexString(param);
        _testOutputHelper.WriteLine(byteString.ToBase64());

        var hex = byteString.ToHex();
        var hex1 = byteString.ToByteArray().ToHex();
        var base64 = byteString.ToBase64();

        return Task.CompletedTask;
    }
}