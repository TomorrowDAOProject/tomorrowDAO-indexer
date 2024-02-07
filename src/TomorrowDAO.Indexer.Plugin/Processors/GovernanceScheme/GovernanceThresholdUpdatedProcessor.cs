using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

public class GovernanceThresholdUpdatedProcessor : GovernanceSchemeProcessorBase<GovernanceThresholdUpdated>
{
    public GovernanceThresholdUpdatedProcessor(
        ILogger<AElfLogEventProcessorBase<GovernanceThresholdUpdated, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
        IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository) :
        base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, governanceSubSchemeRepository)
    {
    }

    protected override async Task HandleEventAsync(GovernanceThresholdUpdated eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var subSchemeId = eventValue.SubSchemeId.ToHex();
        Logger.LogInformation(
            "[GovernanceThresholdUpdated] start subSchemeId:{subSchemeId} chainId:{chainId} ",
            subSchemeId, chainId);
        var governanceSubSchemeIndex =
            await GovernanceSubSchemeRepository.GetFromBlockStateSetAsync(subSchemeId, context.ChainId);
        if (governanceSubSchemeIndex == null)
        {
            Logger.LogInformation(
                "[GovernanceThresholdUpdated] governanceSchemeIndex with id {id} chainId {chainId} has not existed.",
                subSchemeId, chainId);
            return;
        }
        governanceSubSchemeIndex.OfThreshold(eventValue.SchemeThresholdUpdate);
        await SaveIndexAsync(governanceSubSchemeIndex, context);
        Logger.LogInformation(
            "[GovernanceThresholdUpdated] end subSchemeId:{subSchemeId} chainId:{chainId} ",
            subSchemeId, chainId);
    }
}