using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Shouldly;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Organization;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

public class OrganizationProcessorTests : TomorrowDAOIndexerPluginTestBase
{
    protected readonly IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo>
        _organizationRepository;
    
    public OrganizationProcessorTests()
    {
        _organizationRepository = GetRequiredService<IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo>>();
    }
    
    [Fact]
    public async Task OrganizationCreated_Test()
    {
        var processor = GetRequiredService<OrganizationCreatedProcessor>();

        var logEvent = new OrganizationCreated
        {
            OrganizationHash = HashHelper.ComputeFrom(OrganizationAddress),
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            GovernanceSchemeId = HashHelper.ComputeFrom(Id2),
            OrganizationName = "Organization Test",
            Symbol = Elf
        };
        await MockEventProcess(logEvent.ToLogEvent(), processor);
        var index =
            await _organizationRepository.GetFromBlockStateSetAsync(OrganizationAddress, ChainAelf);
        index.ShouldNotBeNull();
        index.Id.ShouldBe(OrganizationAddress);
        index.OrganizationAddress.ShouldBe(OrganizationAddress);
        index.Symbol.ShouldBe(Elf);
    }
    
    [Fact]
    public async Task MemberAdded_Test()
    {
        await OrganizationCreated_Test();
        
        var processor = GetRequiredService<MemberAddedProcessor>();
        var logEvent = new MemberAdded
        {
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            MemberList = new AddressList
            {
                Value = { Address.FromBase58(ExecuteAddress) }
            }
        };
        await MockEventProcess(logEvent.ToLogEvent(), processor);
        var index =
            await _organizationRepository.GetFromBlockStateSetAsync(OrganizationAddress, ChainAelf);
        index.OrganizationMemberSet.ShouldContain(ExecuteAddress);
    }
    
    [Fact]
    public async Task MemberChanged_Test()
    {
        await OrganizationCreated_Test();
        await MemberAdded_Test();
        var processor = GetRequiredService<MemberChangedProcessor>();
        var logEvent = new MemberChanged
        {
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            OldMember = Address.FromBase58(ExecuteAddress),
            NewMember = Address.FromBase58(ExecuteAddressNew)
        };
        await MockEventProcess(logEvent.ToLogEvent(), processor);
        var index =
            await _organizationRepository.GetFromBlockStateSetAsync(OrganizationAddress, ChainAelf);
        index.OrganizationMemberSet.ShouldContain(ExecuteAddressNew);
        index.OrganizationMemberSet.ShouldNotContain(ExecuteAddress);
    }
    
    [Fact]
    public async Task MemberRemoved_Test()
    {
        await OrganizationCreated_Test();
        await MemberAdded_Test();
        var processor = GetRequiredService<MemberRemovedProcessor>();
        var logEvent = new MemberRemoved
        {
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            MemberList = new AddressList
            {
                Value = { Address.FromBase58(ExecuteAddress) }
            }
        };
        await MockEventProcess(logEvent.ToLogEvent(), processor);
        var index =
            await _organizationRepository.GetFromBlockStateSetAsync(OrganizationAddress, ChainAelf);
        index.OrganizationMemberSet.ShouldNotContain(ExecuteAddress);
    }
}