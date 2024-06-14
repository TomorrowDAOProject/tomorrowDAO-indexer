using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Goverenace;

[CollectionDefinition(ClusterCollection.Name)]
public class GovernanceSchemeProcessorTests : TomorrowDAOIndexerPluginTestBase
{
     [Fact]
     public async Task GovernanceSchemeAdded_Test()
     {
         var id = IdGenerateHelper.GetId(ChainAelf, DAOId, SchemeAddress);
         await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
         var governanceSchemeIndex = await GovernanceSchemeRepository.GetFromBlockStateSetAsync(id, ChainAelf);
         governanceSchemeIndex.ShouldNotBeNull();
         governanceSchemeIndex.Id.ShouldBe(id);
         governanceSchemeIndex.DAOId.ShouldBe(DAOId);
         governanceSchemeIndex.SchemeId.ShouldBe(SchemeId);
         governanceSchemeIndex.SchemeAddress.ShouldBe(SchemeAddress);
         governanceSchemeIndex.GovernanceToken.ShouldBe(Elf);
         governanceSchemeIndex.GovernanceMechanism.ShouldBe(GovernanceMechanism.Referendum);
         governanceSchemeIndex.MinimalRequiredThreshold.ShouldBe(10);
         governanceSchemeIndex.MinimalVoteThreshold.ShouldBe(12);
         governanceSchemeIndex.MinimalApproveThreshold.ShouldBe(50);
         governanceSchemeIndex.MaximalRejectionThreshold.ShouldBe(30);
         governanceSchemeIndex.MaximalAbstentionThreshold.ShouldBe(20);
         governanceSchemeIndex.ProposalThreshold.ShouldBe(10);
         
         await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
     }
     
     [Fact]
     public async Task GovernanceSchemeThresholdRemoved_Test()
     {
         await MockEventProcess(GovernanceSchemeThresholdRemoved(), GovernanceSchemeThresholdRemovedProcessor);
         
         var id = IdGenerateHelper.GetId(ChainAelf, DAOId, SchemeAddress);
         await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
         await MockEventProcess(GovernanceSchemeThresholdRemoved(), GovernanceSchemeThresholdRemovedProcessor);
         var governanceSchemeIndex = await GovernanceSchemeRepository.GetFromBlockStateSetAsync(id, ChainAelf);
         governanceSchemeIndex.ShouldBeNull();
     }
     
     [Fact]
     public async Task GovernanceSchemeThresholdUpdated_Test()
     {
         await MockEventProcess(GovernanceSchemeThresholdUpdated(), GovernanceSchemeThresholdUpdatedProcessor);
         
         await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
         await MockEventProcess(GovernanceSchemeThresholdUpdated(), GovernanceSchemeThresholdUpdatedProcessor);
         var id = IdGenerateHelper.GetId(ChainAelf, DAOId, SchemeAddress);
         var governanceSchemeIndex = await GovernanceSchemeRepository.GetFromBlockStateSetAsync(id, ChainAelf);
         governanceSchemeIndex.ShouldNotBeNull();
         governanceSchemeIndex.MinimalRequiredThreshold.ShouldBe(1);
         governanceSchemeIndex.MinimalVoteThreshold.ShouldBe(1);
         governanceSchemeIndex.MinimalApproveThreshold.ShouldBe(1);
         governanceSchemeIndex.MaximalRejectionThreshold.ShouldBe(1);
         governanceSchemeIndex.MaximalAbstentionThreshold.ShouldBe(1);
     }

     [Fact]
     public async Task GovernanceTokenSet_Test()
     {
         await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
         await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
         await MockEventProcess(GovernanceTokenSet(), GovernanceTokenSetProcessor);
         
         var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
         DAOIndex.ShouldNotBeNull();
         DAOIndex.GovernanceToken.ShouldBe("USDT");
     }
}