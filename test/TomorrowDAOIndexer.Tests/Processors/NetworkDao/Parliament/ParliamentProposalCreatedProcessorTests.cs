using AElf;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;
using Xunit.Abstractions;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

public class ParliamentProposalCreatedProcessorTests : TomorrowDAOIndexerTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ParliamentProposalCreatedProcessorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task ProposalCreated_Test()
    {
        var logEvent = NetworkDaoProposalCreate();
        await ParliamentProposalCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var id = IdGenerateHelper.GetId(ChainId, proposalId);
        var networkDaoProposalIndex = await GetIndexById<NetworkDaoProposalIndex>(id);
        networkDaoProposalIndex.ShouldNotBeNull();
        networkDaoProposalIndex.OrgType.ShouldBe(NetworkDaoOrgType.Parliament);
        networkDaoProposalIndex.ProposalId.ShouldBe(proposalId);
        networkDaoProposalIndex.Metadata.ChainId.ShouldBe(ChainId);
    }
}