using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateRemovedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        await MockEventProcess(CandidateRemoved(), CandidateRemovedProcessor);

        var electionIndex = await GetIndexById<ElectionIndex>(IdGenerateHelper.GetId(ChainId, DAOId, DAOCreator, 0));
        electionIndex.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_ElectionNotExist_Test()
    {
        await MockEventProcess(CandidateRemoved(), CandidateRemovedProcessor);
    }
}