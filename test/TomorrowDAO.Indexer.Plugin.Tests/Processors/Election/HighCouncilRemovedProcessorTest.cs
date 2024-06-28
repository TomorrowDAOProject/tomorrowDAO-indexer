using AElf;
using AElf.Types;
using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Entities;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

[CollectionDefinition(ClusterCollection.Name)]
public class HighCouncilRemovedProcessorTest : TomorrowDAOIndexerPluginTestBase
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

        logEvent = HighCouncilRemoved();
        await MockEventProcess(logEvent, HighCouncilRemovedProcessor);
        
        highCouncilConfig =
            await ElectionHighCouncilConfigRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.Count.ShouldBe(1);
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotContain(User);
    }
}