using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
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
        var votingItemIndex = await GetIndexById<ElectionVotingItemIndex>(id);
        votingItemIndex.ShouldNotBeNull();
        
        
        var highCouncilConfigIndex = await GetIndexById<ElectionHighCouncilConfigIndex>(id);
        highCouncilConfigIndex.ShouldNotBeNull();
    }
}