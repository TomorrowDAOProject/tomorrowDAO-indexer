using AElfIndexer.Client.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class SubsistStatusSetProcessor : DAOProcessorBase<SubsistStatusSet>
{
    public SubsistStatusSetProcessor(ILogger<DAOProcessorBase<SubsistStatusSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(SubsistStatusSet eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var subsistStatus = eventValue.Status;
        Logger.LogInformation("[SubsistStatusSet] START: Id={Id}, ChainId={ChainId}, SubsistStatus={SubsistStatus}",
            DAOId, chainId, subsistStatus);
        try
        {
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[SubsistStatusSet] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.SubsistStatus = subsistStatus;
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[SubsistStatusSet] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[SubsistStatusSet] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}