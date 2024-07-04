using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Election;

public class ElectionVotingEventRegisteredProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var logEvent = ElectionVotingEventRegistered();
        await MockEventProcess(logEvent, ElectionVotingEventRegisteredProcessor);

        var id = IdGenerateHelper.GetId(HashHelper.ComputeFrom(Id1).ToHex(), ChainId);
        var votingItemIndex = await GetIndexById(id, ElectionVotingItemRepository);
        votingItemIndex.ShouldNotBeNull();
        
        
        var highCouncilConfigIndex = await GetIndexById(id, ElectionHighCouncilConfigRepository);
        highCouncilConfigIndex.ShouldNotBeNull();
    }
}