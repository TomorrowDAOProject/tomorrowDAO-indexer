using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Treasury;

public class PausedProcessor : DAOProcessorBase<Paused>
{
    public PausedProcessor(ILogger<DAOProcessorBase<Paused>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAORepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(Paused eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[Paused] START: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[Paused] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            ObjectMapper.Map(eventValue, DAOIndex);
            DAOIndex.IsTreasuryPause = true;
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[Paused] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Paused] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}