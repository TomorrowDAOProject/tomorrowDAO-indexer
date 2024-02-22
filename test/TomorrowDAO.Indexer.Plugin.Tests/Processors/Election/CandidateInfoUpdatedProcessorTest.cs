using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

public class CandidateInfoUpdatedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        await MockEventProcess(CandidateInfoUpdated(), CandidateInfoUpdatedProcessor);

        var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper.GetId(ChainAelf, DAOId, DAOCreator, 0, HighCouncilType.Candidate), ChainAelf);
        electionIndex.ShouldBeNull();
        var electionIndex1 = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper.GetId(ChainAelf, DAOId, DAOCreator, 0, HighCouncilType.BlackList), ChainAelf);
        electionIndex1.ShouldNotBeNull();
        electionIndex1.DAOId.ShouldBe(DAOId);
        electionIndex1.TermNumber.ShouldBe(0);
        electionIndex1.HighCouncilType.ShouldBe(HighCouncilType.BlackList);
        electionIndex1.Address.ShouldBe(DAOCreator);
    }
}