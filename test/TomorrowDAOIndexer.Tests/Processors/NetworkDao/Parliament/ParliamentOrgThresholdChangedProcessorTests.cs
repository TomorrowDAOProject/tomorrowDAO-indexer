using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

public class ParliamentOrgThresholdChangedProcessorTests : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task ProposalCreated_Test()
    {
        var logEvent = NetworkDaoOrgThresholdChanged();
        await ParliamentOrgThresholdChangedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var id = IdGenerateHelper.GetId(ChainId, TransactionId);
        var index = await GetIndexById<NetworkDaoOrgThresholdChangedIndex>(id);
        index.ShouldNotBeNull();
        index.OrgType.ShouldBe(NetworkDaoOrgType.Parliament);
        index.OrganizationAddress.ShouldBe(OrganizationAddress);
        index.MinimalApprovalThreshold.ShouldBe(10);
        index.MinimalVoteThreshold.ShouldBe(10);
        index.MaximalAbstentionThreshold.ShouldBe(10);
        index.MaximalRejectionThreshold.ShouldBe(10);
        index.Id.ShouldNotBeNull();
        index.Metadata.ShouldNotBeNull();
        index.Metadata.ChainId.ShouldNotBeNull();
        index.Metadata.Block.ShouldNotBeNull();
        index.Metadata.Block.BlockHash.ShouldNotBeNull();
        index.Metadata.Block.BlockHeight.ShouldBe(index.BlockHeight);

        var orgChangedIndex =
            await GetIndexById<NetworkDaoOrgChangedIndex>(IdGenerateHelper.GetId(ChainId, OrganizationAddress));
        orgChangedIndex.ShouldNotBeNull();
        orgChangedIndex.BlockHeight.ShouldBeGreaterThan(0);
    }
}