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

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class EmergencyTransferredProcessor : TreasuryProcessorBase<EmergencyTransferred>
{
    public EmergencyTransferredProcessor(ILogger<DAOProcessorBase<EmergencyTransferred>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository, IDAOProvider DAOProvider) 
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(EmergencyTransferred eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[EmergencyTransferred] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var symbol = eventValue.Symbol;
            var id = IdGenerateHelper.GetId(chainId, DAOId, symbol);
            var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(id, chainId);
            if (treasuryFundIndex == null)
            {
                Logger.LogInformation("[EmergencyTransferred] TreasuryFund not support symbol: Id={Id}, ChainId={ChainId}, Symbol={Symbol}", 
                    id, chainId, symbol);
                return;
            }
            treasuryFundIndex.LockedFunds -= eventValue.Amount;
            await SaveIndexAsync(treasuryFundIndex, context);
            var executor = eventValue.Account.ToBase58();
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            await SaveIndexAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.TransactionId, executor, TreasuryRecordType.Donate),
                DAOId = DAOId,
                Executor = executor,
                From = DAOIndex?.TreasuryAccountAddress,
                To = eventValue.Recipient?.ToBase58(),
                Amount = eventValue.Amount,
                Symbol = symbol,
                TreasuryRecordType = TreasuryRecordType.EmergencyTransfer,
                CreateTime = context.BlockTime
            }, context);
            Logger.LogInformation("[EmergencyTransferred] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[EmergencyTransferred] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}