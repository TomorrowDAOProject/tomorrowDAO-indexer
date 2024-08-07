using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateInfoUpdatedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        await MockEventProcess(CandidateInfoUpdated(), CandidateInfoUpdatedProcessor);

        var electionIndex = await GetIndexById<ElectionIndex>(IdGenerateHelper.GetId(ChainId, DAOId, DAOCreator, 0));
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