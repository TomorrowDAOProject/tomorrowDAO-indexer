using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;
using Xunit.Abstractions;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class ProposalCreatedProcessorTest : TomorrowDAOIndexerTestBase
{
    
    private readonly ITestOutputHelper _testOutputHelper;

    public ProposalCreatedProcessorTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        await MockEventProcess(ProposalCreated_Veto(), ProposalCreatedProcessor);
        await MockEventProcess(ProposalCreated(), ProposalCreatedProcessor);
        
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex = await GetIndexById<ProposalIndex>(proposalId);
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
        proposalIndex.GovernanceMechanism.ShouldBe(GovernanceMechanism.Organization);
        proposalIndex.MinimalRequiredThreshold.ShouldBe(10);
        proposalIndex.MinimalVoteThreshold.ShouldBe(12);
        proposalIndex.MinimalApproveThreshold.ShouldBe(50);
        proposalIndex.MaximalRejectionThreshold.ShouldBe(30);
        proposalIndex.MaximalAbstentionThreshold.ShouldBe(20);
        proposalIndex.ProposalThreshold.ShouldBe(10);
        proposalIndex.ActiveTimePeriod.ShouldBe(1);
        proposalIndex.VetoActiveTimePeriod.ShouldBe(5);
        proposalIndex.PendingTimePeriod.ShouldBe(3);
        proposalIndex.ExecuteTimePeriod.ShouldBe(2);
        proposalIndex.VetoExecuteTimePeriod.ShouldBe(4);
        var transaction = proposalIndex.Transaction;
        transaction.ShouldNotBeNull();
        transaction.ContractMethodName.ShouldBe("ContractMethodName");
        transaction.ToAddress.ShouldBe(ExecuteAddress);
        
        var vetoProposalIndex = await GetIndexById<ProposalIndex>(VetoProposalId);
        vetoProposalIndex.ProposalStatus.ShouldBe(ProposalStatus.Challenged);
        vetoProposalIndex.ProposalStage.ShouldBe(ProposalStage.Pending);
        
        var latestParticipatedIndex = await GetIndexById<LatestParticipatedIndex>(IdGenerateHelper.GetId(ChainId, DAOCreator));
        latestParticipatedIndex.ShouldNotBeNull();
        latestParticipatedIndex.Address.ShouldBe(DAOCreator);
        latestParticipatedIndex.DAOId.ShouldBe(DAOId);
        latestParticipatedIndex.ParticipatedType.ShouldBe(ParticipatedType.Proposed);
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