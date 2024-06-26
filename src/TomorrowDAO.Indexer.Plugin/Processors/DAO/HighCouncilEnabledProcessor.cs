using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class HighCouncilEnabledProcessor : DAOProcessorBase<HighCouncilEnabled>
{
    public HighCouncilEnabledProcessor(ILogger<AElfLogEventProcessorBase<HighCouncilEnabled, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider)
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilEnabled eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilEnabled] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAOProvider.GetDaoAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[HighCouncilEnabled] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                DAOIndex = ObjectMapper.Map<HighCouncilEnabled, DAOIndex>(eventValue);
            }
            else
            {
                ObjectMapper.Map(eventValue, DAOIndex);
            }
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[HighCouncilEnabled] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilEnabled] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}