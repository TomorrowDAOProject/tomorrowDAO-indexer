using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

public class CandidateAddedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);

        var electionId = IdGenerateHelper.GetId(ChainAelf, DAOId, DAOCreator, 0, HighCouncilType.Candidate);
        var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(electionId, ChainAelf);
        electionIndex.ShouldNotBeNull();
        electionIndex.Id.ShouldBe(electionId);
        electionIndex.DAOId.ShouldBe(DAOId);
        electionIndex.TermNumber.ShouldBe(0);
        electionIndex.HighCouncilType.ShouldBe(HighCouncilType.Candidate);
        electionIndex.IsRemoved.ShouldBe(false);
        electionIndex.Address.ShouldBe(DAOCreator);
    }
}