using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Shouldly;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

public class GovernanceSchemeProcessorTests : TomorrowDAOIndexerPluginTestBase
{
//
//     protected readonly IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo>
//         _governanceSchemeRepository;
//
//     protected readonly IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo>
//         _governanceSubSchemeRepository;
//
//
//     public GovernanceSchemeProcessorTests()
//     {
//         _governanceSchemeRepository =
//             GetRequiredService<IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo>>();
//
//         _governanceSubSchemeRepository =
//             GetRequiredService<IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo>>();
//     }
//
//     [Fact]
//     public async Task GovernanceSchemeCreated_Test()
//     {
//         var processor = GetRequiredService<GovernanceSchemeCreatedProcessor>();
//
//         var logEvent = new GovernanceSchemeCreated
//         {
//             GovernanceSchemeId = HashHelper.ComputeFrom(Id2),
//             GovernanceMechanism = GovernanceMechanism.Parliament,
//             Creator = Address.FromBase58(Creator),
//             SchemeThreshold = new Contracts.Governance.GovernanceSchemeThreshold
//             {
//                 MinimalRequiredThreshold = 10,
//                 MinimalVoteThreshold = 12,
//                 MinimalApproveThreshold = 50,
//                 MaximalRejectionThreshold = 30,
//                 MaximalAbstentionThreshold = 20
//             }
//         };
//         await MockEventProcess(logEvent.ToLogEvent(), processor);
//         var governanceSchemeIndex =
//             await _governanceSchemeRepository.GetFromBlockStateSetAsync(GovernanceSchemeId, ChainAelf);
//         governanceSchemeIndex.ShouldNotBeNull();
//         governanceSchemeIndex.Id.ShouldBe(GovernanceSchemeId);
//         governanceSchemeIndex.GovernanceSchemeId.ShouldBe(GovernanceSchemeId);
//         governanceSchemeIndex.GovernanceMechanism.ShouldBe(Enums.GovernanceMechanism.Parliament);
//     }
//     
//     [Fact]
//     public async Task GovernanceSubSchemeAdded_Test()
//     {
//         await GovernanceSchemeCreated_Test();
//         
//         var processor = GetRequiredService<GovernanceSubSchemeAddedProcessor>();
//
//         var logEvent = new GovernanceSubSchemeAdded
//         {
//             ParentSchemeId = HashHelper.ComputeFrom(Id2),
//             SubSchemeId = HashHelper.ComputeFrom(SubId),
//             SchemeThreshold = new Contracts.Governance.GovernanceSchemeThreshold
//             {
//                 MinimalRequiredThreshold = 11,
//                 MinimalVoteThreshold = 13,
//                 MinimalApproveThreshold = 50,
//                 MaximalRejectionThreshold = 30,
//                 MaximalAbstentionThreshold = 20
//             }
//         };
//         await MockEventProcess(logEvent.ToLogEvent(), processor);
//         var subSchemeId = HashHelper.ComputeFrom(SubId).ToHex();
//         var governanceSubSchemeIndex =
//             await _governanceSubSchemeRepository.GetFromBlockStateSetAsync( subSchemeId, ChainAelf);
//         governanceSubSchemeIndex.ShouldNotBeNull();
//         governanceSubSchemeIndex.Id.ShouldBe(subSchemeId);
//         governanceSubSchemeIndex.SubSchemeId.ShouldBe(subSchemeId);
//         governanceSubSchemeIndex.GovernanceMechanism.ShouldBe(Enums.GovernanceMechanism.Parliament);
//         governanceSubSchemeIndex.MinimalRequiredThreshold.ShouldBe(11);
//         governanceSubSchemeIndex.MinimalVoteThreshold.ShouldBe(13);
//     }
//     
//     [Fact]
//     public async Task GovernanceSubSchemeRemoved_Test()
//     {
//         await GovernanceSchemeCreated_Test();
//         
//         var processor = GetRequiredService<GovernanceSubSchemeRemovedProcessor>();
//
//         var logEvent = new GovernanceSubSchemeRemoved
//         {
//             ParentSchemeId = HashHelper.ComputeFrom(Id2),
//             SubSchemeId = HashHelper.ComputeFrom(SubId)
//         };
//         await MockEventProcess(logEvent.ToLogEvent(), processor);
//         var subSchemeId = HashHelper.ComputeFrom(SubId).ToHex();
//         var governanceSchemeIndex =
//             await _governanceSubSchemeRepository.GetFromBlockStateSetAsync( subSchemeId, ChainAelf);
//         governanceSchemeIndex.ShouldBeNull();
//     }
//
//     [Fact]
//     public async Task GovernanceSubSchemeUpdated_Test()
//     {
//         await GovernanceSubSchemeAdded_Test();
//
//         var processor = GetRequiredService<GovernanceSubSchemeUpdatedProcessor>();
//
//         var logEvent = new GovernanceSubSchemeUpdated
//         {
//             UpdateSchemeId = HashHelper.ComputeFrom(Id2),
//             SubSchemeId = HashHelper.ComputeFrom(SubId),
//             SchemeThresholdUpdate = new Contracts.Governance.GovernanceSchemeThreshold
//             {
//                 MinimalRequiredThreshold = 16,
//                 MinimalVoteThreshold = 17,
//                 MinimalApproveThreshold = 50,
//                 MaximalRejectionThreshold = 30,
//                 MaximalAbstentionThreshold = 20
//             }
//         };
//         await MockEventProcess(logEvent.ToLogEvent(), processor);
//         var subSchemeId = HashHelper.ComputeFrom(SubId).ToHex();
//         var governanceSubSchemeIndex =
//             await _governanceSubSchemeRepository.GetFromBlockStateSetAsync(subSchemeId, ChainAelf);
//         governanceSubSchemeIndex.ShouldNotBeNull();
//         governanceSubSchemeIndex.Id.ShouldBe(subSchemeId);
//         governanceSubSchemeIndex.SubSchemeId.ShouldBe(subSchemeId);
//         governanceSubSchemeIndex.GovernanceMechanism.ShouldBe(Enums.GovernanceMechanism.Parliament);
//         governanceSubSchemeIndex.MinimalRequiredThreshold.ShouldBe(16);
//         governanceSubSchemeIndex.MinimalVoteThreshold.ShouldBe(17);
//     }
//     
//     [Fact]
//     public async Task GovernanceThresholdUpdated_Test()
//     {
//         await GovernanceSubSchemeAdded_Test();
//
//         var processor = GetRequiredService<GovernanceThresholdUpdatedProcessor>();
//
//         var logEvent = new GovernanceThresholdUpdated
//         {
//             SubSchemeId = HashHelper.ComputeFrom(SubId),
//             SchemeThresholdUpdate = new Contracts.Governance.GovernanceSchemeThreshold
//             {
//                 MinimalRequiredThreshold = 16,
//                 MinimalVoteThreshold = 17,
//                 MinimalApproveThreshold = 50,
//                 MaximalRejectionThreshold = 30,
//                 MaximalAbstentionThreshold = 20
//             }
//         };
//         await MockEventProcess(logEvent.ToLogEvent(), processor);
//         var subSchemeId = HashHelper.ComputeFrom(SubId).ToHex();
//         var governanceSubSchemeIndex =
//             await _governanceSubSchemeRepository.GetFromBlockStateSetAsync(subSchemeId, ChainAelf);
//         governanceSubSchemeIndex.ShouldNotBeNull();
//         governanceSubSchemeIndex.Id.ShouldBe(subSchemeId);
//         governanceSubSchemeIndex.SubSchemeId.ShouldBe(subSchemeId);
//         governanceSubSchemeIndex.GovernanceMechanism.ShouldBe(Enums.GovernanceMechanism.Parliament);
//         governanceSubSchemeIndex.MinimalRequiredThreshold.ShouldBe(16);
//         governanceSubSchemeIndex.MinimalVoteThreshold.ShouldBe(17);
//         governanceSubSchemeIndex.MinimalApproveThreshold.ShouldBe(50);
//         governanceSubSchemeIndex.MaximalRejectionThreshold.ShouldBe(30);
//         governanceSubSchemeIndex.MaximalAbstentionThreshold.ShouldBe(20);
//     }
}