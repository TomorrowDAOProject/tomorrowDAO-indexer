using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.DAO;

public class MetadataUpdatedProcessor : DAOProcessorBase<MetadataUpdated>
{
    public override async Task ProcessAsync(MetadataUpdated logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[MetadataUpdated] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[MetadataUpdated] DAO not already existed: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex = ObjectMapper.Map(logEvent, DAOIndex);
            await SaveEntityAsync(DAOIndex, context);
            Logger.LogInformation("[MetadataUpdated] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[MetadataUpdated] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}