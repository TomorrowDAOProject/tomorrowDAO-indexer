using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Treasury;

public class DonationReceivedProcessor : TreasuryProcessorBase<DonationReceived>
{
    public DonationReceivedProcessor(ILogger<AElfLogEventProcessorBase<DonationReceived, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository,
        IDAOProvider DAOProvider)
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(DonationReceived eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[DonationReceived] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var symbol = eventValue.Symbol;
            var id = IdGenerateHelper.GetId(chainId, DAOId, symbol);
            var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(id, chainId);
            if (treasuryFundIndex == null)
            {
                Logger.LogInformation("[DonationReceived] TreasuryFund not support symbol: Id={Id}, ChainId={ChainId}, Symbol={Symbol}", 
                    DAOId, chainId, symbol);
                return;
            }
            treasuryFundIndex.AvailableFunds += eventValue.Amount;
            await SaveIndexAsync(treasuryFundIndex, context);
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            var executor = eventValue.Donor?.ToBase58();
            await SaveIndexAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.TransactionId, executor, TreasuryRecordType.Donate),
                DAOId = DAOId,
                Executor = executor,
                FromAddress = executor,
                ToAddress = DAOIndex?.TreasuryAccountAddress,
                Amount = eventValue.Amount,
                Symbol = symbol,
                TreasuryRecordType = TreasuryRecordType.Donate,
                CreateTime = context.BlockTime
            }, context);
            
            Logger.LogInformation("[DonationReceived] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[DonationReceived] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}