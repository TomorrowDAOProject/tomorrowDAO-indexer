using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Election;

public class HighCouncilRemovedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var logEvent = HighCouncilAdded();
        await MockEventProcess(logEvent, HighCouncilAddedProcessor);

        var daoId = HashHelper.ComputeFrom(Id1);
        var id = IdGenerateHelper.GetId(daoId.ToHex(), ChainId);
        ElectionHighCouncilConfigIndex highCouncilConfig = await GetIndexById(id, ElectionHighCouncilConfigRepository);
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.Count.ShouldBe(2);
        highCouncilConfig.InitialHighCouncilMembers.ShouldContain(User);

        var highCouncilRemoved = HighCouncilRemoved();
        await MockEventProcess(highCouncilRemoved, HighCouncilRemovedProcessor);
        
        highCouncilConfig = await GetIndexById(id, ElectionHighCouncilConfigRepository);
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotBeNull();
        highCouncilConfig.InitialHighCouncilMembers.Count.ShouldBe(1);
        highCouncilConfig.InitialHighCouncilMembers.ShouldNotContain(User);
    }
}