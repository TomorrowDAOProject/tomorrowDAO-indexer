using AElf;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationReceiptCreatedProcessorTests : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task ProposalCreated_Test()
    {
        await AssociationProposalCreatedProcessor.ProcessAsync(GenerateLogEventContext(NetworkDaoProposalCreate()));
        
        var logEvent = NetworkDaoProposalReceiptCreated();
        await AssociationReceiptCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var proposalId = HashHelper.ComputeFrom(ProposalId).ToHex();
        var id = IdGenerateHelper.GetId(ChainId, TransactionId);
        var index = await GetIndexById<NetworkDaoProposalVoteRecordIndex>(id);
        index.ShouldNotBeNull();
        index.OrgType.ShouldBe(NetworkDaoOrgType.Association);
        index.ProposalId.ShouldBe(proposalId);
        index.OrganizationAddress.ShouldBe(OrganizationAddress);
        index.Amount.ShouldBe(1);
        index.Symbol.ShouldBe(string.Empty);
        index.ReceiptType.ShouldBe(ReceiptTypeEnum.Approve);
        index.Id.ShouldNotBeNull();
        index.Metadata.ShouldNotBeNull();
        index.Metadata.ChainId.ShouldNotBeNull();
        index.Metadata.Block.ShouldNotBeNull();
        index.Metadata.Block.BlockHash.ShouldNotBeNull();
        index.Metadata.Block.BlockHeight.ShouldBe(index.BlockHeight);

        var proposalIndex =
            await GetIndexById<NetworkDaoProposalIndex>(IdGenerateHelper.GetId(ChainId, proposalId));
        proposalIndex.TotalAmount.ShouldBe(1);
    }
}