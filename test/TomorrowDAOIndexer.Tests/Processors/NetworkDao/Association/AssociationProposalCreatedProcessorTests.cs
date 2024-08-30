using AElf;
using AElf.Standards.ACS3;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;
using Xunit.Abstractions;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationProposalCreatedProcessorTests : TomorrowDAOIndexerTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AssociationProposalCreatedProcessorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task ProposalCreated_Test()
    {
        var logEvent = NetworkDaoProposalCreate();
        await AssociationProposalCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var id = IdGenerateHelper.GetId(ChainId, proposalId);
        var networkDaoProposalIndex = await GetIndexById<NetworkDaoProposalIndex>(id);
        networkDaoProposalIndex.ShouldNotBeNull();
        networkDaoProposalIndex.OrgType.ShouldBe(NetworkDaoOrgType.Association);
        networkDaoProposalIndex.ProposalId.ShouldBe(proposalId);
        networkDaoProposalIndex.Metadata.ChainId.ShouldBe(ChainId);
    }
}