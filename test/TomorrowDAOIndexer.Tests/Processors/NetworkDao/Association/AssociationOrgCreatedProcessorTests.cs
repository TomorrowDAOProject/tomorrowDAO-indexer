using AElf.Types;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgCreatedProcessorTests : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task ProposalCreated_Test()
    {
        var logEvent = NetworkDaoOrganizationCreated();
        await AssociationOrgCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var id = IdGenerateHelper.GetId(ChainId, OrganizationAddress);
        var orgCreatedIndex = await GetIndexById<NetworkDaoOrgCreatedIndex>(id);
        orgCreatedIndex.ShouldNotBeNull();
        orgCreatedIndex.OrgType.ShouldBe(NetworkDaoOrgType.Association);
        orgCreatedIndex.OrganizationAddress.ShouldBe(OrganizationAddress);
        orgCreatedIndex.Id.ShouldNotBeNull();
        orgCreatedIndex.Metadata.ShouldNotBeNull();
        orgCreatedIndex.Metadata.ChainId.ShouldNotBeNull();
        orgCreatedIndex.Metadata.Block.ShouldNotBeNull();
        orgCreatedIndex.Metadata.Block.BlockHash.ShouldNotBeNull();
        orgCreatedIndex.Metadata.Block.BlockHeight.ShouldBe(orgCreatedIndex.BlockHeight);
        
        var orgChangedIndex = await GetIndexById<NetworkDaoOrgChangedIndex>(id);
        orgChangedIndex.ShouldNotBeNull();
        orgChangedIndex.BlockHeight.ShouldBeGreaterThan(0);
    }
}