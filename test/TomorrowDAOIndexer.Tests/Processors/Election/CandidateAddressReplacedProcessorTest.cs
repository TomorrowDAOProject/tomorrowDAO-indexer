using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateAddressReplacedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        await MockEventProcess(CandidateAddressReplaced(), CandidateAddressReplacedProcessor);

        var electionIndex = await GetIndexById<ElectionIndex>(IdGenerateHelper.GetId(ChainId, DAOId, Creator, 0));
        electionIndex.ShouldBeNull();
        var electionIndex1 = await GetIndexById<ElectionIndex>(IdGenerateHelper.GetId(ChainId, DAOId, User, 0));
        electionIndex1.ShouldNotBeNull();
        electionIndex1.DAOId.ShouldBe(DAOId);
        electionIndex1.TermNumber.ShouldBe(0);
        electionIndex1.HighCouncilType.ShouldBe(HighCouncilType.Candidate);
        electionIndex1.Address.ShouldBe(User);
    }
    [Fact]
    public async Task HandleEventAsync_ElectionNotExist_Test()
    {
        await MockEventProcess(CandidateAddressReplaced(), CandidateAddressReplacedProcessor);
    }
}