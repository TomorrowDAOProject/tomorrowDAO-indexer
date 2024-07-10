using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;
using Xunit.Abstractions;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.NetworkDao.Association;

[CollectionDefinition(ClusterCollection.Name)]
public class AssociationProposalCreatedProcessorTests : TomorrowDAOIndexerPluginTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AssociationProposalCreatedProcessorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task ProposalCreated_Test()
    {
        await MockEventProcess(NetworkDaoProposalCreate(), AssociationProposalCreatedProcessor);

        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var id = IdGenerateHelper.GetId(ChainAelf, proposalId);
        var networkDaoProposalIndex = await NetworkDaoProposalRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        networkDaoProposalIndex.ShouldNotBeNull();
        networkDaoProposalIndex.ProposalType.ShouldBe(NetworkDaoProposalType.Association);
        networkDaoProposalIndex.ProposalId.ShouldBe(proposalId);
        networkDaoProposalIndex.ChainId.ShouldBe(ChainAelf);
    }
}