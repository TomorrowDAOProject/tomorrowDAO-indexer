using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Treasury;

public class UnpausedProcessor : TreasuryProcessorBase<Unpaused>
{
    public UnpausedProcessor(ILogger<AElfLogEventProcessorBase<Unpaused, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository,
        IDAOProvider DAOProvider)
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(Unpaused eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[Unpaused] START: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var DAOIndex = await DAOProvider.GetDAOAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[Unpaused] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            ObjectMapper.Map(eventValue, DAOIndex);
            DAOIndex.IsTreasuryPause = false;
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[Unpaused] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Unpaused] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}