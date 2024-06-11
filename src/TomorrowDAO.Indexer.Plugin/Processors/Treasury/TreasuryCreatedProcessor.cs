using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Treasury;

public class TreasuryCreatedProcessor : TreasuryProcessorBase<TreasuryCreated>
{
    public TreasuryCreatedProcessor(ILogger<AElfLogEventProcessorBase<TreasuryCreated, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository,
        IDAOProvider DAOProvider)
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(TreasuryCreated eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[TreasuryCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAOProvider.GetDaoAsync(chainId, daoId);
            if (DAOIndex != null)
            {
                Logger.LogInformation("[TreasuryCreated] update treasury address: DAOId={DAOId}, ChainId={ChainId}", daoId, chainId);
                DAOIndex.TreasuryAccountAddress = eventValue.TreasuryAccountAddress.ToBase58();
                await DAOProvider.SaveIndexAsync(DAOIndex, context);
            }
            Logger.LogInformation("[TreasuryCreated] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryCreated] Exception Id={DAOId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}