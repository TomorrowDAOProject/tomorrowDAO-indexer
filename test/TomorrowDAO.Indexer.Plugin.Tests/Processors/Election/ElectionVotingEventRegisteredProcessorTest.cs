using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

[CollectionDefinition(ClusterCollection.Name)]
public class ElectionVotingEventRegisteredProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var logEvent = ElectionVotingEventRegistered();
        await MockEventProcess(logEvent, ElectionVotingEventRegisteredProcessor);

        var id = IdGenerateHelper.GetId(HashHelper.ComputeFrom(Id1).ToHex(), ChainAelf);
        var votingItemIndex = await ElectionVotingItemRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        votingItemIndex.ShouldNotBeNull();
        
        
        var highCouncilConfigIndex = await ElectionHighCouncilConfigRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        highCouncilConfigIndex.ShouldNotBeNull();
    }
}