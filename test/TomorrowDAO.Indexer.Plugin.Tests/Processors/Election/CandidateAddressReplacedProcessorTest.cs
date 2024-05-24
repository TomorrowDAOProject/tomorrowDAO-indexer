using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

public class CandidateAddressReplacedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        await MockEventProcess(CandidateAddressReplaced(), CandidateAddressReplacedProcessor);

        var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper.GetId(ChainAelf, DAOId, Creator, 0), ChainAelf);
        electionIndex.ShouldBeNull();
        var electionIndex1 = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper.GetId(ChainAelf, DAOId, User, 0), ChainAelf);
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