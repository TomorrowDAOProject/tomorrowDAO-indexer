using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

// public class GovernanceSubSchemeUpdatedProcessor : GovernanceSchemeProcessorBase<GovernanceSubSchemeUpdated>
// {
//     public GovernanceSubSchemeUpdatedProcessor(
//         ILogger<AElfLogEventProcessorBase<GovernanceSubSchemeUpdated, LogEventInfo>> logger,
//         IObjectMapper objectMapper,
//         IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
//         IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
//         IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository) :
//         base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, governanceSubSchemeRepository)
//     {
//     }
//
//     protected override async Task HandleEventAsync(GovernanceSubSchemeUpdated eventValue, LogEventContext context)
//     {
//         var chainId = context.ChainId;
//         var subSchemeId = eventValue.SubSchemeId.ToHex();
//         Logger.LogInformation(
//             "[GovernanceSubSchemeUpdated] start subSchemeId:{subSchemeId} chainId:{chainId} ",
//             subSchemeId, chainId);
//         var governanceSubSchemeIndex =
//             await GovernanceSubSchemeRepository.GetFromBlockStateSetAsync(subSchemeId, context.ChainId);
//         if (governanceSubSchemeIndex == null)
//         {
//             Logger.LogInformation(
//                 "[GovernanceSubSchemeUpdated] governanceSchemeIndex with id {id} chainId {chainId} has not existed.",
//                 subSchemeId, chainId);
//             return;
//         }
//         //update governance scheme
//         governanceSubSchemeIndex.ParentSchemeId = eventValue.UpdateSchemeId?.ToHex();
//         //use governanceScheme GovernanceMechanism
//         var governanceSchemeIndex =
//             await GovernanceSchemeRepository.GetFromBlockStateSetAsync(governanceSubSchemeIndex.ParentSchemeId,
//                 context.ChainId);
//         governanceSubSchemeIndex.GovernanceMechanism = governanceSchemeIndex.GovernanceMechanism;
//         governanceSubSchemeIndex.OfThreshold(eventValue.SchemeThresholdUpdate);
//         await SaveIndexAsync(governanceSubSchemeIndex, context);
//         Logger.LogInformation(
//             "[GovernanceSubSchemeUpdated] end subSchemeId:{subSchemeId} chainId:{chainId} ",
//             subSchemeId, chainId);
//     }
// }