using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.DAO;

public class HighCouncilEnabledProcessor : DAOProcessorBase<HighCouncilEnabled>
{
    public override async Task ProcessAsync(HighCouncilEnabled logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilEnabled] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[HighCouncilEnabled] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                DAOIndex = ObjectMapper.Map<HighCouncilEnabled, DAOIndex>(logEvent);
            }
            else
            {
                ObjectMapper.Map(logEvent, DAOIndex);
            }
            await SaveEntityAsync(DAOIndex, context);
            Logger.LogInformation("[HighCouncilEnabled] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilEnabled] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}