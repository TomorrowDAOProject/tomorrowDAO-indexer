using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgWhiteListChangedProcessorTests : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task ProposalCreated_Test()
    {
        var logEvent = NetworkDaoOrgWhiteListChanged();
        await AssociationOrgWhiteListChangedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var id = IdGenerateHelper.GetId(ChainId, TransactionId);
        var index = await GetIndexById<NetworkDaoOrgWhiteListChangedIndex>(id);
        index.ShouldNotBeNull();
        index.OrgType.ShouldBe(NetworkDaoOrgType.Association);
        index.OrganizationAddress.ShouldBe(OrganizationAddress);
        index.ProposerWhiteList.ShouldNotBeNull();
        index.ProposerWhiteList.ShouldContain(User);
        index.ProposerWhiteList.ShouldContain(Creator);
        index.ProposerWhiteList.Count.ShouldBe(2);
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