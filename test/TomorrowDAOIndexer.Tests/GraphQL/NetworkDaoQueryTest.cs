using Shouldly;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public class NetworkDaoQueryTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task GetNetworkDaoOrgChangedIndexAsync_Test()
    {
        var logEvent = NetworkDaoOrganizationCreated();
        await AssociationOrgCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));
        await ParliamentOrgCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));
        await ReferendumOrgCreatedProcessor.ProcessAsync(GenerateLogEventContext(logEvent));

        var pageResultDto = await Query.GetNetworkDaoOrgChangedIndexAsync(NetworkDaoOrgChangedIndexRepository,
            ObjectMapper,
            new GetNetworkDaoDataChangedIndexInput
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(1);
        var daoOrgChangedIndexDto = pageResultDto.Data[0];

        daoOrgChangedIndexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        daoOrgChangedIndexDto.TransactionInfo.ShouldNotBeNull();
        daoOrgChangedIndexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
    }

    [Fact]
    public async Task GetNetworkDaoOrgCreatedIndexAsync_Test()
    {
        await AssociationOrgCreatedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrganizationCreated(OrganizationAddress)));
        await ParliamentOrgCreatedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrganizationCreated(ExecuteAddress)));
        await ReferendumOrgCreatedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrganizationCreated(Creator)));

        var pageResultDto = await Query.GetNetworkDaoOrgCreatedIndexAsync(NetworkDaoOrgCreatedIndexRepository,
            ObjectMapper,
            new GetNetworkDaoDataCreatedIndexInput
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(3);
        var orgCreatedIndexDto = pageResultDto.Data[0];
        orgCreatedIndexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        orgCreatedIndexDto.TransactionInfo.ShouldNotBeNull();
        orgCreatedIndexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
    }

    [Fact]
    public async Task GetNetworkDaoOrgThresholdChangedIndexAsync_Test()
    {
        await AssociationOrgCreatedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrganizationCreated(OrganizationAddress)));
        await AssociationOrgThresholdChangedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrgThresholdChanged(OrganizationAddress)));

        var pageResultDto = await Query.GetNetworkDaoOrgThresholdChangedIndexAsync(
            NetworkDaoOrgThresholdChangedIndexRepository, ObjectMapper,
            new GetNetworkDaoDataThresholdChangedIndexInput()
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(1);
        var indexDto = pageResultDto.Data[0];
        indexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        indexDto.TransactionInfo.ShouldNotBeNull();
        indexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
        indexDto.MaximalAbstentionThreshold = 10;
    }


    [Fact]
    public async Task GetNetworkDaoOrgWhiteListChangedIndexAsync_Test()
    {
        await AssociationOrgCreatedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrganizationCreated(OrganizationAddress)));
        await AssociationOrgWhiteListChangedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrgWhiteListChanged(OrganizationAddress)));

        var pageResultDto = await Query.GetNetworkDaoOrgWhiteListChangedIndexAsync(
            NetworkDaoOrgWhiteListChangedIndexRepository, ObjectMapper,
            new GetNetworkDaoDataWhiteListChangedIndexInput()
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(1);
        var indexDto = pageResultDto.Data[0];
        indexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        indexDto.TransactionInfo.ShouldNotBeNull();
        indexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
        indexDto.ProposerWhiteList.ShouldNotBeNull();
        indexDto.ProposerWhiteList.Count.ShouldBe(2);
        indexDto.ProposerWhiteList.ShouldContain(User);
    }

    [Fact]
    public async Task GetNetworkDaoOrgMemberChangedIndexAsync_Test()
    {
        await AssociationOrgCreatedProcessor.ProcessAsync(
            GenerateLogEventContext(NetworkDaoOrganizationCreated(OrganizationAddress)));
        await MockEventProcess(NetworkDaoOrgMemberAdded(), AssociationOrgMemberAddedProcessor);
        await MockEventProcess(NetworkDaoOrgMemberChanged(), AssociationOrgMemberChangedProcessor);
        await MockEventProcess(NetworkDaoOrgMemberRemoved(), AssociationOrgMemberRemovedProcessor);

        // var pageResultDto = await Query.GetNetworkDaoOrgMemberChangedIndexAsync(NetworkDaoOrgMemberChangedIndexRepository, ObjectMapper,
        //     new GetNetworkDaoDataMemberChangedIndexInput
        //     {
        //         ChainId = ChainId,
        //         OrgAddresses = null,
        //         OrgType = NetworkDaoOrgType.All,
        //         StartBlockHeight = BlockHeight,
        //         EndBlockHeight = BlockHeight + 1,
        //         SkipCount = 0,
        //         MaxResultCount = 10,
        //         ChangeType = OrgMemberChangeTypeEnum.All
        //     });
        // pageResultDto.ShouldNotBeNull();
        // pageResultDto.TotalCount.ShouldBe(3);

        var pageResultDto = await Query.GetNetworkDaoOrgMemberChangedIndexAsync(
            NetworkDaoOrgMemberChangedIndexRepository, ObjectMapper,
            new GetNetworkDaoDataMemberChangedIndexInput
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10,
                ChangeType = OrgMemberChangeTypeEnum.MemberAdded
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(1);

        var indexDto = pageResultDto.Data[0];
        indexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        indexDto.TransactionInfo.ShouldNotBeNull();
        indexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
        indexDto.ChangeType.ShouldBe(OrgMemberChangeTypeEnum.MemberAdded);
    }

    [Fact]
    public async Task GetNetworkDaoProposalIndexAsync_Test()
    {
        var logEvent = NetworkDaoProposalCreate();
        var logEventContext = GenerateLogEventContext(logEvent);
        logEventContext.Transaction.To = TomorrowDAOConst.PortKeyContractAddress1;
        logEventContext.Transaction.MethodName = TomorrowDAOConst.PortKeyContactManagerForwardCall;
        logEventContext.Transaction.Params =
            "CiIKIDR1K9aBxJJGkcp7xb4E3gXfmDopAqpxgEIwWIbZix9DEiIKINuHm2HV0EtgiIS4TNntef1HpWVLx697Z0b0mKKe7MbFGhJNYW5hZ2VyRm9yd2FyZENhbGwiQAoFQUdFTlQQgKCUpY0dGgxJc3N1ZVRlc3RpbmciIgog24ebYdXQS2CIhLhM2e15/UelZUvHr3tnRvSYop7sxsU=";
        await AssociationProposalCreatedProcessor.ProcessAsync(logEventContext);

        var pageResultDto = await Query.GetNetworkDaoProposalIndexAsync(NetworkDaoProposalIndexRepository, ObjectMapper,
            new GetNetworkDaoProposalIndexInput
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10,
                ProposalIds = null,
                Title = null,
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(1);

        var indexDto = pageResultDto.Data[0];
        indexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        indexDto.IsReleased.ShouldBeFalse();
        indexDto.TransactionInfo.ShouldNotBeNull();
        indexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
        indexDto.TransactionInfo.IsAAForwardCall.ShouldBeTrue();
        indexDto.TransactionInfo.To.ShouldBe(TomorrowDAOConst.PortKeyContractAddress1);
        indexDto.TransactionInfo.RealTo.ShouldBe("2fgbLE3pTghxVLmo5iR63pm2sZYe3vkphCG1Sungg3ed9sdsaQ");
    }

    [Fact]
    public async Task GetNetworkDaoProposalReleasedIndexAsync_Test()
    {
        var logEvent = NetworkDaoProposalCreate();
        var logEventContext = GenerateLogEventContext(logEvent);
        logEventContext.Transaction.To = TomorrowDAOConst.PortKeyContractAddress1;
        logEventContext.Transaction.MethodName = TomorrowDAOConst.PortKeyContactManagerForwardCall;
        logEventContext.Transaction.Params =
            "CiIKIDR1K9aBxJJGkcp7xb4E3gXfmDopAqpxgEIwWIbZix9DEiIKINuHm2HV0EtgiIS4TNntef1HpWVLx697Z0b0mKKe7MbFGhJNYW5hZ2VyRm9yd2FyZENhbGwiQAoFQUdFTlQQgKCUpY0dGgxJc3N1ZVRlc3RpbmciIgog24ebYdXQS2CIhLhM2e15/UelZUvHr3tnRvSYop7sxsU=";
        await AssociationProposalCreatedProcessor.ProcessAsync(logEventContext);

        await AssociationProposalReleasedProcessor.ProcessAsync(GenerateLogEventContext(NetworkDaoProposalReleased()));

        var pageResultDto = await Query.GetNetworkDaoProposalReleasedIndexAsync(
            NetworkDaoProposalReleasedIndexRepository, ObjectMapper,
            new GetNetworkDaoProposalReleasedIndexInput
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10,
                ProposalIds = null,
                Title = null,
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(1);

        var indexDto = pageResultDto.Data[0];
        indexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        indexDto.TransactionInfo.ShouldNotBeNull();
        indexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
        indexDto.TransactionInfo.IsAAForwardCall.ShouldBeFalse();

        var proposalPageResultDto = await Query.GetNetworkDaoProposalIndexAsync(NetworkDaoProposalIndexRepository,
            ObjectMapper,
            new GetNetworkDaoProposalIndexInput
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10,
                ProposalIds = null,
                Title = null,
            });
        proposalPageResultDto.ShouldNotBeNull();
        proposalPageResultDto.TotalCount.ShouldBe(1);
        var proposalIndexDto = proposalPageResultDto.Data[0];
        proposalIndexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        proposalIndexDto.IsReleased.ShouldBeTrue();
        proposalIndexDto.TransactionInfo.ShouldNotBeNull();
        proposalIndexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
        proposalIndexDto.TransactionInfo.IsAAForwardCall.ShouldBeTrue();
        proposalIndexDto.TransactionInfo.To.ShouldBe(TomorrowDAOConst.PortKeyContractAddress1);
        proposalIndexDto.TransactionInfo.RealTo.ShouldBe("2fgbLE3pTghxVLmo5iR63pm2sZYe3vkphCG1Sungg3ed9sdsaQ");
    }

    [Fact]
    public async Task GetNetworkDaoProposalVoteRecordIndexAsync_Test()
    {
        await AssociationProposalCreatedProcessor.ProcessAsync(GenerateLogEventContext(NetworkDaoProposalCreate()));
        await AssociationReceiptCreatedProcessor.ProcessAsync(GenerateLogEventContext(NetworkDaoProposalReceiptCreated()));

        var pageResultDto = await Query.GetNetworkDaoProposalVoteRecordIndexAsync(NetworkDaoProposalVoteRecordIndexRepository,
            ObjectMapper,
            new GetNetworkDaoProposalVoteRecordIndexInput
            {
                ChainId = ChainId,
                OrgAddresses = null,
                OrgType = NetworkDaoOrgType.All,
                StartBlockHeight = BlockHeight,
                EndBlockHeight = BlockHeight + 1,
                SkipCount = 0,
                MaxResultCount = 10,
                ProposalIds = null
            });
        pageResultDto.ShouldNotBeNull();
        pageResultDto.TotalCount.ShouldBe(1);

        var indexDto = pageResultDto.Data[0];
        indexDto.OrganizationAddress.ShouldBe(OrganizationAddress);
        indexDto.ReceiptType.ShouldBe(ReceiptTypeEnum.Approve);
        indexDto.TransactionInfo.ShouldNotBeNull();
        indexDto.TransactionInfo.TransactionId.ShouldBe(TransactionId);
        indexDto.TransactionInfo.IsAAForwardCall.ShouldBeFalse();
    }
}