using AElf;
using AElf.Types;
using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Entities;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

[CollectionDefinition(ClusterCollection.Name)]
public class HighCouncilAddedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var logEvent = HighCouncilAdded();
        await MockEventProcess(logEvent, HighCouncilAddedProcessor);

        var daoId = HashHelper.ComputeFrom(Id1);
        var id = IdGenerateHelper.GetId(daoId.ToHex(), ChainAelf);
        ElectionHighCouncilConfigIndex highCouncilConfig =
            await ElectionHighCouncilConfigRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.Count.ShouldBe(2);
        highCouncilConfig.InitialHighCouncilMembers.ShouldContain(User);
    }
    
    [Fact]
    public async Task HandleEventAsync_Test_OpenElected()
    {
        var daoId = HashHelper.ComputeFrom(Id1);
        var id = IdGenerateHelper.GetId(daoId.ToHex(), ChainAelf);
        
        var logEvent = ElectionVotingEventRegistered();
        await MockEventProcess(logEvent, ElectionVotingEventRegisteredProcessor);

        ElectionHighCouncilConfigIndex highCouncilConfig =
            await ElectionHighCouncilConfigRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.Count.ShouldBe(0);
        
        logEvent = HighCouncilAdded();
        await MockEventProcess(logEvent, HighCouncilAddedProcessor);
        highCouncilConfig =
            await ElectionHighCouncilConfigRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.Count.ShouldBe(2);
        highCouncilConfig.InitialHighCouncilMembers.ShouldContain(User);
    }
}