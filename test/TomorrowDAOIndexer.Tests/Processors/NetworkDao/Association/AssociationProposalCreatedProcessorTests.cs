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
        var logEventContext = GenerateLogEventContext(logEvent);
        logEventContext.Transaction.To = TomorrowDAOConst.PortKeyContractAddress1;
        logEventContext.Transaction.MethodName = TomorrowDAOConst.PortKeyContactManagerForwardCall;
        logEventContext.Transaction.Params =
            "CiIKIDR1K9aBxJJGkcp7xb4E3gXfmDopAqpxgEIwWIbZix9DEiIKINuHm2HV0EtgiIS4TNntef1HpWVLx697Z0b0mKKe7MbFGhJNYW5hZ2VyRm9yd2FyZENhbGwiQAoFQUdFTlQQgKCUpY0dGgxJc3N1ZVRlc3RpbmciIgog24ebYdXQS2CIhLhM2e15/UelZUvHr3tnRvSYop7sxsU=";
        await AssociationProposalCreatedProcessor.ProcessAsync(logEventContext);

        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var id = IdGenerateHelper.GetId(ChainId, proposalId);
        var networkDaoProposalIndex = await GetIndexById<NetworkDaoProposalIndex>(id);
        networkDaoProposalIndex.ShouldNotBeNull();
        networkDaoProposalIndex.OrgType.ShouldBe(NetworkDaoOrgType.Association);
        networkDaoProposalIndex.ProposalId.ShouldBe(proposalId);
        networkDaoProposalIndex.Metadata.ChainId.ShouldBe(ChainId);
    }
}