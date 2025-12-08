using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.DAO;

public class HighCouncilDisabledProcessor : DAOProcessorBase<HighCouncilDisabled>
{
    public override async Task ProcessAsync(HighCouncilDisabled logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilDisabled] START: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[HighCouncilDisabled] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.IsHighCouncilEnabled = false;
            await SaveEntityAsync(DAOIndex, context);
            Logger.LogInformation("[HighCouncilDisabled] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilDisabled] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}