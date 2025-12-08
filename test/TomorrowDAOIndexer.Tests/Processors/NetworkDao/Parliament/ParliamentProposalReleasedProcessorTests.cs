using AElf;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

public class ParliamentProposalReleasedProcessorTests : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task ProposalCreated_Test()
    {
        await ParliamentProposalCreatedProcessor.ProcessAsync(GenerateLogEventContext(NetworkDaoProposalCreate()));
        
        var logEvent = NetworkDaoProposalReleased();
        await ParliamentProposalReleasedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var id = IdGenerateHelper.GetId(ChainId, proposalId);
        var index = await GetIndexById<NetworkDaoProposalIndex>(id);
        index.ShouldNotBeNull();
        index.OrgType.ShouldBe(NetworkDaoOrgType.Parliament);
        index.ProposalId.ShouldBe(proposalId);
        index.OrganizationAddress.ShouldBe(OrganizationAddress);
        index.Title.ShouldBe(ProposalTitle);
        index.Description.ShouldBe(ProposalDescription);
        index.IsReleased.ShouldBe(true);
        index.Id.ShouldNotBeNull();
        index.Metadata.ShouldNotBeNull();
        index.Metadata.ChainId.ShouldNotBeNull();
        index.Metadata.Block.ShouldNotBeNull();
        index.Metadata.Block.BlockHash.ShouldNotBeNull();
        index.Metadata.Block.BlockHeight.ShouldBe(index.BlockHeight);

        var orgChangedIndex =
            await GetIndexById<NetworkDaoOrgChangedIndex>(IdGenerateHelper.GetId(ChainId, OrganizationAddress));
        orgChangedIndex.ShouldBeNull();
    }
}