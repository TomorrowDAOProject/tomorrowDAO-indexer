using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class ElectionQueryTest : QueryTestBase
{
    [Fact]
    public async Task GetElectionListAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        
        var elections = await Query.GetElectionListAsync(ElectionRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            ChainId = ChainAelf,
            StartBlockHeight = BlockHeight,
            EndBlockHeight = BlockHeight + 1,
            MaxResultCount = 10
        });
        elections.ShouldNotBeNull();
        elections.Count.ShouldBe(1);
        var electionDto = elections[0];
        electionDto.ChainId.ShouldBe(ChainAelf);
        electionDto.DAOId.ShouldBe(DAOId);
        electionDto.TermNumber.ShouldBe(0);
        electionDto.HighCouncilType.ShouldBe(HighCouncilType.Candidate);
        electionDto.Address.ShouldBe(DAOCreator);
    }

    [Fact]
    public async Task GetHighCouncilListAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);
        
        var elections = await Query.GetHighCouncilListAsync(ElectionRepository, ObjectMapper, new GetHighCouncilListInput
        {
            DAOId = DAOId,
            ChainId = ChainAelf,
            MaxResultCount = 1,
            SkipCount = 0,
            TermNumber = 0,
            HighCouncilType = HighCouncilType.Candidate.ToString()
        });
        elections.ShouldNotBeNull();
        elections.TotalCount.ShouldBe(1);
        elections.DataList.Count.ShouldBe(1);
        var electionDto = elections.DataList[0];
        electionDto.ChainId.ShouldBe(ChainAelf);
        electionDto.DAOId.ShouldBe(DAOId);
        electionDto.TermNumber.ShouldBe(0);
        electionDto.HighCouncilType.ShouldBe(HighCouncilType.Candidate);
        electionDto.Address.ShouldBe(DAOCreator);
    }
}