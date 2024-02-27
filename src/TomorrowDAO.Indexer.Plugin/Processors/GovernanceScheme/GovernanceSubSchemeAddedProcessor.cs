using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

// public class GovernanceSubSchemeAddedProcessor : GovernanceSchemeProcessorBase<GovernanceSubSchemeAdded>
// {
//     public GovernanceSubSchemeAddedProcessor(
//         ILogger<AElfLogEventProcessorBase<GovernanceSubSchemeAdded, LogEventInfo>> logger,
//         IObjectMapper objectMapper,
//         IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
//         IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
//         IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository) :
//         base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, governanceSubSchemeRepository)
//     {
//     }
//
//     protected override async Task HandleEventAsync(GovernanceSubSchemeAdded eventValue, LogEventContext context)
//     {
//         var chainId = context.ChainId;
//         var subSchemeId = eventValue.SubSchemeId.ToHex();
//         Logger.LogInformation(
//             "[GovernanceSubSchemeAdded] start subSchemeId:{subSchemeId} chainId:{chainId} ",
//             subSchemeId, chainId);
//         var governanceSubSchemeIndex =
//             await GovernanceSubSchemeRepository.GetFromBlockStateSetAsync(subSchemeId, context.ChainId);
//         if (governanceSubSchemeIndex != null)
//         {
//             Logger.LogInformation(
//                 "[GovernanceSubSchemeAdded] governanceSchemeIndex with id {id} chainId {chainId} has existed.",
//                 subSchemeId, chainId);
//             return;
//         }
//
//         governanceSubSchemeIndex = ObjectMapper.Map<GovernanceSubSchemeAdded, GovernanceSubSchemeIndex>(eventValue);
//         governanceSubSchemeIndex.Id = subSchemeId;
//         governanceSubSchemeIndex.CreateTime = context.BlockTime;
//         governanceSubSchemeIndex.OfThreshold(eventValue.SchemeThreshold);
//         //use governanceScheme GovernanceMechanism
//         var governanceSchemeIndex =
//             await GovernanceSchemeRepository.GetFromBlockStateSetAsync(governanceSubSchemeIndex.ParentSchemeId,
//                 context.ChainId);
//         governanceSubSchemeIndex.GovernanceMechanism = governanceSchemeIndex.GovernanceMechanism;
//
//         await SaveIndexAsync(governanceSubSchemeIndex, context);
//         Logger.LogInformation(
//             "[GovernanceSubSchemeAdded] end subSchemeId:{subSchemeId} chainId:{chainId} ",
//             subSchemeId, chainId);
//     }
// }