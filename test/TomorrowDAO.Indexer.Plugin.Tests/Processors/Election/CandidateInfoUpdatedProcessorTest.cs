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

        var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper.GetId(ChainAelf, DAOId, DAOCreator, 0), ChainAelf);
        electionIndex.ShouldNotBeNull();
        electionIndex.DAOId.ShouldBe(DAOId);
        electionIndex.TermNumber.ShouldBe(0);
        electionIndex.HighCouncilType.ShouldBe(HighCouncilType.BlackList);
        electionIndex.Address.ShouldBe(DAOCreator);
    }

    [Fact]
    public async Task HandleEventAsync_ElectionNotExist_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
    }
}