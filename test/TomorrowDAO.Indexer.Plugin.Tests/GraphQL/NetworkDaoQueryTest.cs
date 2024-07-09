using AElf;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class NetworkDaoQueryTest : QueryTestBase
{
    [Fact]
    public async Task GetNetworkDaoProposalsAsync_Test()
    {
        var proposalIdAStr = "proposalIdA";
        var proposalAId = HashHelper.ComputeFrom(proposalIdAStr).ToHex();
        await MockEventProcess(NetworkDaoProposalCreate(proposalIdAStr), AssociationProposalCreatedProcessor);
        
        var proposalIdBStr = "proposalIdB";
        var proposalBId = HashHelper.ComputeFrom(proposalIdBStr).ToHex();
        await MockEventProcess(NetworkDaoProposalCreate(proposalIdBStr), ParliamentProposalCreatedProcessor);
        
        var proposalIdCStr = "proposalIdC";
        var proposalCId = HashHelper.ComputeFrom(proposalIdCStr).ToHex();
        await MockEventProcess(NetworkDaoProposalCreate(proposalIdCStr), ReferendumProposalCreatedProcessor);

        var proposalIds = new List<string>() { proposalAId, proposalBId, proposalCId };

        var result = await Query.GetNetworkDaoProposalsAsync(NetworkDaoProposalRepository, ObjectMapper,
            new GetNetworkDaoProposalsInput
            {
                ChainId = ChainAelf,
                ProposalIds = proposalIds,
            });
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(3);
        result.Items.Count.ShouldBe(3);
        result.Items.First().ProposalId.ShouldNotBeNull();
        result.Items.First().BlockHash.ShouldNotBeNull();
        result.Items.First().ChainId.ShouldNotBeNull();
        
        result = await Query.GetNetworkDaoProposalsAsync(NetworkDaoProposalRepository, ObjectMapper,
            new GetNetworkDaoProposalsInput
            {
                ChainId = ChainAelf,
                ProposalIds = proposalIds,
                ProposalType = NetworkDaoProposalType.Association
            });
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(1);
        result.Items.Count.ShouldBe(1);
        result.Items.First().ProposalId.ShouldNotBeNull();
        result.Items.First().BlockHash.ShouldNotBeNull();
        result.Items.First().ChainId.ShouldNotBeNull();
        result.Items.First().ProposalType.ShouldBe((int)NetworkDaoProposalType.Association);
        result.Items.First().ProposalId.ShouldBe(proposalAId);
    }
}