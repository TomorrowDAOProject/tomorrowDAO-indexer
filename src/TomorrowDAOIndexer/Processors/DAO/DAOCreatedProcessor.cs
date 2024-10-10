using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.DAO;

public class DAOCreatedProcessor : DAOProcessorBase<DAOCreated>
{
    public override async Task ProcessAsync(DAOCreated logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId?.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[DAOCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex != null)
            {
                Logger.LogInformation("[DAOCreated] DAO already existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                DAOIndex = ObjectMapper.Map(logEvent, DAOIndex);
            }
            else
            {
                DAOIndex = ObjectMapper.Map<DAOCreated, DAOIndex>(logEvent);
            }
            DAOIndex.OfPeriod();
            DAOIndex.CreateTime = context.Block.BlockTime;
            await SaveEntityAsync(DAOIndex, context);
            Logger.LogInformation("[DAOCreated] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[DAOCreated] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}