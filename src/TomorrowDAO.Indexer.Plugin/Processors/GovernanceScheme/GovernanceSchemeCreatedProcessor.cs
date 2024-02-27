using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

// public class GovernanceSchemeCreatedProcessor : GovernanceSchemeProcessorBase<GovernanceSchemeCreated>
// {
//     public GovernanceSchemeCreatedProcessor(
//         ILogger<AElfLogEventProcessorBase<GovernanceSchemeCreated, LogEventInfo>> logger,
//         IObjectMapper objectMapper,
//         IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
//         IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
//         IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository) :
//         base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, governanceSubSchemeRepository)
//     {
//     }
//
//     protected override async Task HandleEventAsync(GovernanceSchemeCreated eventValue, LogEventContext context)
//     {
//         var chainId = context.ChainId;
//         var governanceSchemeId = eventValue.GovernanceSchemeId.ToHex();
//         Logger.LogInformation(
//             "[GovernanceSchemeCreated] start governanceSchemeId:{governanceSchemeId} chainId:{chainId} ",
//             governanceSchemeId, chainId);
//         var governanceSchemeIndex =
//             await GovernanceSchemeRepository.GetFromBlockStateSetAsync(governanceSchemeId, context.ChainId);
//         if (governanceSchemeIndex != null)
//         {
//             Logger.LogInformation(
//                 "[GovernanceSchemeCreated] governanceSchemeIndex with id {id} chainId {chainId} has existed.",
//                 governanceSchemeId, chainId);
//             return;
//         }
//         governanceSchemeIndex = ObjectMapper.Map<GovernanceSchemeCreated, GovernanceSchemeIndex>(eventValue);
//         governanceSchemeIndex.Id = governanceSchemeId;
//         governanceSchemeIndex.CreateTime = context.BlockTime;
//         governanceSchemeIndex.OfThreshold(eventValue.SchemeThreshold);
//         await SaveIndexAsync(governanceSchemeIndex, context);
//         Logger.LogInformation(
//             "[GovernanceSchemeCreated] end governanceSchemeId:{governanceSchemeId} chainId:{chainId} ",
//             governanceSchemeId, chainId);
//     }
// }