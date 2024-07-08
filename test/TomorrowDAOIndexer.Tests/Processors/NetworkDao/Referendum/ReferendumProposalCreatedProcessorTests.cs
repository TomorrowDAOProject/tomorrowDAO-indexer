using AElf;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;
using Xunit.Abstractions;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Referendum;

public class ReferendumProposalCreatedProcessorTests : TomorrowDAOIndexerTestBase
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ReferendumProposalCreatedProcessorTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public async Task ProposalCreated_Test()
    {
        var logEvent = NetworkDaoProposalCreate();
        await ReferendumProposalCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var id = IdGenerateHelper.GetId(ChainId, proposalId);
        var networkDaoProposalIndex = await GetIndexById<NetworkDaoProposalIndex>(id);
        networkDaoProposalIndex.ShouldNotBeNull();
        networkDaoProposalIndex.ProposalType.ShouldBe(NetworkDaoProposalType.Referendum);
        networkDaoProposalIndex.ProposalId.ShouldBe(proposalId);
        networkDaoProposalIndex.Metadata.ChainId.ShouldBe(ChainId);
    }
}