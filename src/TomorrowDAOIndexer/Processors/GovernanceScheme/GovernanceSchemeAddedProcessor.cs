using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceSchemeAddedProcessor : GovernanceProcessorBase<GovernanceSchemeAdded>
{ 
    public override async Task ProcessAsync(GovernanceSchemeAdded logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var schemeAddress = logEvent.SchemeAddress?.ToBase58();
        var DAOId = logEvent.DaoId?.ToHex();
        var id = IdGenerateHelper.GetId(chainId, DAOId, schemeAddress);
        Logger.LogInformation("[GovernanceSchemeAdded] start id {id}", id);
        try
        {
            var governanceSchemeIndex = await GetEntityAsync<GovernanceSchemeIndex>(id);
            if (governanceSchemeIndex != null)
            {
                Logger.LogInformation("[GovernanceSchemeAdded] GovernanceScheme already existed id {id}", id);
                return;
            }
            governanceSchemeIndex = ObjectMapper.Map<GovernanceSchemeAdded, GovernanceSchemeIndex>(logEvent);
            governanceSchemeIndex.Id = id;
            governanceSchemeIndex.OfThreshold(logEvent.SchemeThreshold);
            governanceSchemeIndex.CreateTime = context.Block.BlockTime;
            await SaveEntityAsync(governanceSchemeIndex, context);
            Logger.LogInformation("[GovernanceSchemeAdded] end id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[GovernanceSchemeAdded] Exception Id={id}", id);
            throw;
        }
    }
}