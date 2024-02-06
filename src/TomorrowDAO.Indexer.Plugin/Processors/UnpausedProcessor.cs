using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class UnpausedProcessor : DAOProcessorBase<Unpaused>
{
    public UnpausedProcessor(ILogger<DAOProcessorBase<Unpaused>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAORepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(Unpaused eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[Unpaused] START: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[Unpaused] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.IsTreasuryPause = false;
            DAOIndex.TreasuryPauseExecutor = eventValue.Account.ToBase58();
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[Unpaused] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Unpaused] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}