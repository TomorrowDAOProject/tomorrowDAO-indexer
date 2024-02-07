using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Standards.ACS3;
using AElf.Types;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Google.Protobuf.WellKnownTypes;
using Shouldly;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Proposal;
using Xunit;
using ProposalCreated = TomorrowDAO.Contracts.Governance.ProposalCreated;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

public class ProposalProcessorTests : GovernanceSchemeProcessorTests
{

    protected readonly IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo>
        _proposalRepository;


    public ProposalProcessorTests()
    {
        _proposalRepository =
            GetRequiredService<IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo>>();
    }

    [Fact]
    public async Task ProposalCreated_Test()
    {
        await GovernanceSubSchemeAdded_Test();
        
        var processor = GetRequiredService<ProposalCreatedProcessor>();

        var logEvent = new ProposalCreated
        {
            ProposalId = HashHelper.ComputeFrom(ProposalId),
            DaoId = HashHelper.ComputeFrom(Id1),
            GovernanceSchemeId = HashHelper.ComputeFrom(SubId),
            StartTime = Timestamp.FromDateTime(DateTime.UtcNow),
            EndTime = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(2)),
            ExpiredTime = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(3)),
            ProposalStatus = ProposalStatus.Active,
            ProposalType = ProposalType.Governance,
            ExecuteByHighCouncil = false,
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            ExecuteAddress =  Address.FromBase58(ExecuteAddress),
            ProposalTitle = "Proposal Title test",
            ProposalDescription = ProposalDescription,
            Transaction = new ExecuteTransaction()
            {
                ToAddress =  Address.FromBase58(ExecuteContractAddress),
                ContractMethodName = "ForWord"
            }
        };
        await MockEventProcess(logEvent.ToLogEvent(), processor);
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex =
            await _proposalRepository.GetFromBlockStateSetAsync(proposalId, ChainAelf);
        proposalIndex.ShouldNotBeNull();
        proposalIndex.Id.ShouldBe(proposalId);
        proposalIndex.ExecuteAddress.ShouldBe(ExecuteAddress);
        proposalIndex.ExecuteByHighCouncil.ShouldBe(false);
        proposalIndex.TransactionInfo.ToAddress.ShouldBe(ExecuteContractAddress);
        proposalIndex.GovernanceMechanism.ShouldBe(Enums.GovernanceMechanism.Parliament);
        proposalIndex.MinimalRequiredThreshold.ShouldBe(11);
        proposalIndex.MinimalVoteThreshold.ShouldBe(13);
        proposalIndex.MaximalRejectionThreshold.ShouldBe(30);
        proposalIndex.MaximalAbstentionThreshold.ShouldBe(20);
        proposalIndex.MinimalApproveThreshold.ShouldBe(50);
    }
    
    [Fact]
    public async Task ProposalExecuted_Test()
    {
        await ProposalCreated_Test();
        var processor = GetRequiredService<ProposalExecutedProcessor>();
        var executeTime = DateTime.UtcNow.AddDays(2.5);
        var logEvent = new ProposalExecuted
        {
            ProposalId = HashHelper.ComputeFrom(ProposalId),
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            ExecuteTime = Timestamp.FromDateTime(executeTime)
        };
        await MockEventProcess(logEvent.ToLogEvent(), processor);
        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var proposalIndex =
            await _proposalRepository.GetFromBlockStateSetAsync(proposalId, ChainAelf);
        proposalIndex.ProposalStatus.ShouldBe(Enums.ProposalStatus.Executed);
        proposalIndex.ExecuteTime.ShouldBe(executeTime);
    }
}