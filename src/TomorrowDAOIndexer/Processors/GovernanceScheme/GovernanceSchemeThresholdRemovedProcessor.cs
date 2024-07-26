using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceSchemeThresholdRemovedProcessor : GovernanceProcessorBase<GovernanceSchemeThresholdRemoved>
{
    public override async Task ProcessAsync(GovernanceSchemeThresholdRemoved logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var schemeAddress = logEvent.SchemeAddress?.ToBase58();
        var DAOId = logEvent.DaoId?.ToHex();
        var id = IdGenerateHelper.GetId(chainId, DAOId, schemeAddress);
        Logger.LogInformation("[GovernanceSchemeThresholdRemoved] start id {id}", id);
        try
        {
            var governanceSchemeIndex = await GetEntityAsync<GovernanceSchemeIndex>(id);
            if (governanceSchemeIndex == null)
            {
                Logger.LogInformation("[GovernanceSchemeThresholdRemoved] GovernanceScheme not existed id {id}", id);
                return;
            }
            await DeleteEntityAsyncAndCheck<GovernanceSchemeIndex>(id);
            Logger.LogInformation("[GovernanceSchemeThresholdRemoved] end id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[GovernanceSchemeThresholdRemoved] Exception Id={id}", id);
            throw;
        }
    }
}