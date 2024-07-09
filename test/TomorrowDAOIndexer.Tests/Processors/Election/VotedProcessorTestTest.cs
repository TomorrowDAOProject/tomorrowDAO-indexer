using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Election;

public class VotedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        await MockEventProcess(Voted(), VotedProcessor);

        var electionId = IdGenerateHelper.GetId(ChainId, DAOId, DAOCreator, 0);
        var electionIndex = await  GetIndexById<ElectionIndex>(electionId);
        electionIndex.ShouldNotBeNull();
        electionIndex.Id.ShouldBe(electionId);
        electionIndex.DAOId.ShouldBe(DAOId);
        electionIndex.TermNumber.ShouldBe(0);
        electionIndex.HighCouncilType.ShouldBe(HighCouncilType.Candidate);
        electionIndex.Address.ShouldBe(DAOCreator);
    }
}