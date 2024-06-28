using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Election;

[CollectionDefinition(ClusterCollection.Name)]
public class CandidateElectedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var daoId = HashHelper.ComputeFrom(Id1);
        var termNumber = 1L;
        
        await MockEventProcess(CandidateElected(), CandidateElectedProcessor);

        var electionId = IdGenerateHelper.GetId(daoId.ToHex(), termNumber, ChainAelf);
        var candidateElectedIndex = await CandidateElectedRepository.GetFromBlockStateSetAsync(electionId, ChainAelf);
        candidateElectedIndex.ShouldNotBeNull();
        candidateElectedIndex.PreTermNumber.ShouldBe(termNumber);
        candidateElectedIndex.DaoId.ShouldBe(daoId.ToHex());
    }
}