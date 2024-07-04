using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateElectedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var daoId = HashHelper.ComputeFrom(Id1);
        var termNumber = 1L;
        
        await MockEventProcess(CandidateElected(), CandidateElectedProcessor);

        var electionId = IdGenerateHelper.GetId(daoId.ToHex(), termNumber, ChainId);
        var candidateElectedIndex = await GetIndexById<ElectionIndex>(electionId);
        candidateElectedIndex.ShouldNotBeNull();
        candidateElectedIndex.TermNumber.ShouldBe(termNumber);
        candidateElectedIndex.DAOId.ShouldBe(daoId.ToHex());
    }
}