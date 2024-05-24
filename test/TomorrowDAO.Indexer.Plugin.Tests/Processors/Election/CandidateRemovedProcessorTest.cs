using Shouldly;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

public class CandidateRemovedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        await MockEventProcess(CandidateRemoved(), CandidateRemovedProcessor);

        var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper.GetId(ChainAelf, DAOId, DAOCreator, 0), ChainAelf);
        electionIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_ElectionNotExist_Test()
    {
        await MockEventProcess(CandidateRemoved(), CandidateRemovedProcessor);
    }
}