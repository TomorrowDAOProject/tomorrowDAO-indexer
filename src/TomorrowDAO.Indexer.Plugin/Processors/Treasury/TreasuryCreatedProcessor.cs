using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
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
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[TreasuryCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[TreasuryCreated] dao not existed: DAOId={DAOId}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.TreasuryAccountAddress = eventValue.TreasuryAccountAddress.ToBase58();
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            await AddSymbolList(eventValue.SymbolList?.Data.ToList(), DAOId, context);
            Logger.LogInformation("[TreasuryCreated] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryCreated] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}