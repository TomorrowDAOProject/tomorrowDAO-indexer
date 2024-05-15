using AElf;
using Shouldly;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

public class ElectionVotingEventRegisteredProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var logEvent = ElectionVotingEventRegistered();
        await MockEventProcess(logEvent, ElectionVotingEventRegisteredProcessor);

        var id = IdGenerateHelper.GetId(HashHelper.ComputeFrom(Id1), ChainAelf);
        var votingItemIndex = await ElectionVotingItemRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        votingItemIndex.ShouldNotBeNull();
        
        
        var highCouncilConfigIndex = await ElectionHighCouncilConfigRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        highCouncilConfigIndex.ShouldNotBeNull();
    }
}