using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgMemberRemovedProcessorTests : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task ProposalCreated_Test()
    {
        await MockEventProcess(NetworkDaoOrgMemberRemoved(), AssociationOrgMemberRemovedProcessor);
        var id = IdGenerateHelper.GetId(ChainId, TransactionId);
        var orgMemberChangeIndex = await GetIndexById<NetworkDaoOrgMemberChangedIndex>(id);
        orgMemberChangeIndex.ShouldNotBeNull();
        orgMemberChangeIndex.OrgType.ShouldBe(NetworkDaoOrgType.Association);
        orgMemberChangeIndex.OrganizationAddress.ShouldBe(OrganizationAddress);
        orgMemberChangeIndex.AddedAddress.ShouldBeNull();
        orgMemberChangeIndex.RemovedAddress.ShouldBe(User);
        orgMemberChangeIndex.ChangeType.ShouldBe(OrgMemberChangeTypeEnum.MemberRemoved);
        orgMemberChangeIndex.Id.ShouldNotBeNull();
        orgMemberChangeIndex.Metadata.ShouldNotBeNull();
        orgMemberChangeIndex.Metadata.ChainId.ShouldNotBeNull();
        orgMemberChangeIndex.Metadata.Block.ShouldNotBeNull();
        orgMemberChangeIndex.Metadata.Block.BlockHash.ShouldNotBeNull();
        orgMemberChangeIndex.Metadata.Block.BlockHeight.ShouldBe(orgMemberChangeIndex.BlockHeight);
        
        var orgChangedIndex = await GetIndexById<NetworkDaoOrgChangedIndex>(IdGenerateHelper.GetId(ChainId, OrganizationAddress));
        orgChangedIndex.ShouldNotBeNull();
        orgChangedIndex.BlockHeight.ShouldBeGreaterThan(0);
    }
}