using AElf;
using Shouldly;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public class ElectionQueryTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task GetElectionHighCouncilConfigAsync_Test()
    {
        var daoId = HashHelper.ComputeFrom(Id1);

        var logEvent = HighCouncilAdded();
        await MockEventProcess(logEvent, HighCouncilAddedProcessor);

        var result = await Query.GetElectionHighCouncilConfigAsync(ElectionHighCouncilConfigRepository, ObjectMapper,
            new GetElectionHighCouncilListInput
            {
                ChainId = ChainId, StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1,
                DaoId = daoId.ToHex(),
                SkipCount = 0,
                MaxResultCount = 10
            });
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldNotBeNull();
        result.Items.FirstOrDefault().ShouldNotBeNull();
        result.Items.FirstOrDefault()!.InitialHighCouncilMembers.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetElectionVotingItemIndexAsync_Test()
    {
        var daoId = HashHelper.ComputeFrom(Id1);

        var logEvent = ElectionVotingEventRegistered();
        await MockEventProcess(logEvent, ElectionVotingEventRegisteredProcessor);

        var result = await Query.GetElectionVotingItemIndexAsync(ElectionVotingItemRepository, ObjectMapper,
            new GetElectionVotingItemIndexInput
            {
                ChainId = ChainId, StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1,
                DaoId = daoId.ToHex(),
                SkipCount = 0,
                MaxResultCount = 10
            });
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldNotBeNull();
        result.Items.FirstOrDefault().ShouldNotBeNull();
        result.Items.FirstOrDefault()!.DaoId.ShouldBe(daoId.ToHex());
    }
    
    [Fact]
    
    public async Task GetElectionCandidateElectedAsync_Test()
    {
        var daoId = HashHelper.ComputeFrom(Id1);
        var termNumber = 1L;
        
        await MockEventProcess(CandidateElected(), CandidateElectedProcessor);

        var result = await Query.GetElectionCandidateElectedAsync(CandidateElectedRepository, ObjectMapper,
            new GetElectionCandidateElectedInput()
            {
                ChainId = ChainId, StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1,
                DaoId = daoId.ToHex(),
                SkipCount = 0,
                MaxResultCount = 10
            });
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(1);
        result.Items.ShouldNotBeNull();
        result.Items.FirstOrDefault().ShouldNotBeNull();
        result.Items.FirstOrDefault()!.PreTermNumber.ShouldBe(termNumber);
    }


    [Fact]
    public async Task GetElectionListAsync_Test()
    {
        await MockEventProcess(CandidateAdded(), CandidateAddedProcessor);

        var elections = await Query.GetElectionListAsync(ElectionRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            ChainId = ChainId,
            StartBlockHeight = BlockHeight,
            EndBlockHeight = BlockHeight + 1,
            MaxResultCount = 10
        });
        elections.ShouldNotBeNull();
        elections.Count.ShouldBe(1);
        var electionDto = elections[0];
        electionDto.ChainId.ShouldBe(ChainId);
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
            ChainId = ChainId,
            MaxResultCount = 1,
            SkipCount = 0,
            TermNumber = 0,
            HighCouncilType = HighCouncilType.Candidate.ToString()
        });
        elections.ShouldNotBeNull();
        elections.TotalCount.ShouldBe(1);
        elections.DataList.Count.ShouldBe(1);
        var electionDto = elections.DataList[0];
        electionDto.ChainId.ShouldBe(ChainId);
        electionDto.DAOId.ShouldBe(DAOId);
        electionDto.TermNumber.ShouldBe(0);
        electionDto.HighCouncilType.ShouldBe(HighCouncilType.Candidate);
        electionDto.Address.ShouldBe(DAOCreator);
    }
}