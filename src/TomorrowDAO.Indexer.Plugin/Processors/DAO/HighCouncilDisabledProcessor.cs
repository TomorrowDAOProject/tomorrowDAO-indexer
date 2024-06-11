using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class HighCouncilDisabledProcessor : DAOProcessorBase<HighCouncilDisabled>
{
    public HighCouncilDisabledProcessor(ILogger<AElfLogEventProcessorBase<HighCouncilDisabled, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider)
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilDisabled eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilDisabled] START: Id={Id}, ChainId={ChainId}",
            DAOId, chainId);
        try
        {
            var DAOIndex = await DAOProvider.GetDaoAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[HighCouncilDisabled] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.IsHighCouncilEnabled = false;
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[HighCouncilDisabled] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilDisabled] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}