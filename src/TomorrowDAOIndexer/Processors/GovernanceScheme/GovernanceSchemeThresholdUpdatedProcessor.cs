using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceSchemeThresholdUpdatedProcessor : GovernanceProcessorBase<GovernanceSchemeThresholdUpdated>
{
    public override async Task ProcessAsync(GovernanceSchemeThresholdUpdated logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var schemeAddress = logEvent.SchemeAddress?.ToBase58();
        var DAOId = logEvent.DaoId?.ToHex();
        var id = IdGenerateHelper.GetId(chainId, DAOId, schemeAddress);
        Logger.LogInformation("[GovernanceSchemeThresholdUpdated] start id {id}", id);
        try
        {
            var governanceSchemeIndex = await GetEntityAsync<GovernanceSchemeIndex>(id);
            if (governanceSchemeIndex == null)
            {
                Logger.LogInformation("[GovernanceSchemeThresholdUpdated] GovernanceScheme not existed id {id}", id);
                return;
            }
            governanceSchemeIndex.OfThreshold(logEvent.UpdateSchemeThreshold);
            await SaveEntityAsync(governanceSchemeIndex, context);
            Logger.LogInformation("[GovernanceSchemeThresholdUpdated] end id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[GovernanceSchemeThresholdUpdated] Exception Id={id}", id);
            throw;
        }
    }
}