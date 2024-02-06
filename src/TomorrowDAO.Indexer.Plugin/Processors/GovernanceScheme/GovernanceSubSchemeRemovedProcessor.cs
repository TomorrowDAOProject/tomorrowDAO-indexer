using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

public class GovernanceSubSchemeRemovedProcessor : GovernanceSchemeProcessorBase<GovernanceSubSchemeRemoved>
{
    public GovernanceSubSchemeRemovedProcessor(
        ILogger<AElfLogEventProcessorBase<GovernanceSubSchemeRemoved, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
        IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository) :
        base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, governanceSubSchemeRepository)
    {
    }

    protected override async Task HandleEventAsync(GovernanceSubSchemeRemoved eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var subSchemeId = eventValue.SubSchemeId.ToHex();
        Logger.LogInformation(
            "[GovernanceSubSchemeRemoved] start subSchemeId:{subSchemeId} chainId:{chainId} ",
            subSchemeId, chainId);
        var governanceSubSchemeIndex =
            await GovernanceSubSchemeRepository.GetFromBlockStateSetAsync(subSchemeId, context.ChainId);
        if (governanceSubSchemeIndex == null)
        {
            Logger.LogInformation(
                "[GovernanceSubSchemeRemoved] governanceSchemeIndex with id {id} chainId {chainId} has not existed.",
                subSchemeId, chainId);
            return;
        }
        await GovernanceSubSchemeRepository.DeleteAsync(governanceSubSchemeIndex);
        Logger.LogInformation(
            "[GovernanceSubSchemeRemoved] end subSchemeId:{subSchemeId} chainId:{chainId} ",
            subSchemeId, chainId);
    }
}