using AElf.Contracts.MultiToken;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Token;

public class TransferredProcessor : TokenProcessorBase<Transferred>
{
    public TransferredProcessor(ILogger<AElfLogEventProcessorBase<Transferred, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository,
        IDAOProvider DAOProvider)
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(Transferred eventValue, LogEventContext context)
    {
        var treasuryAddress = eventValue.To.ToBase58();
        var chainId = context.ChainId;

        Logger.LogInformation("[Transferred] START: to={treasuryAddress}, ChainId={ChainId}, Event={Event}",
            treasuryAddress, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var daoIndex = await DaoProvider.GetDaoByTreasuryAddressAsync(chainId, treasuryAddress);
            if (daoIndex == null)
            {
                Logger.LogInformation(
                    "[Transferred] to address is not treasury address, to ={treasuryAddress}, ChainId={ChainId}",
                    treasuryAddress, chainId);
                return;
            }

            var daoId = daoIndex.Id;
            var symbol = eventValue.Symbol;

            await CreateOrUpdateTreasuryFundIndex(chainId, daoId, symbol, eventValue, context);

            var executor = eventValue.From?.ToBase58();
            await SaveIndexAsync(new TreasuryRecordIndex
            {
                Id = IdGenerateHelper.GetId(chainId, context.TransactionId, executor, TreasuryRecordType.Deposit),
                DaoId = daoId,
                TreasuryAddress = treasuryAddress,
                Executor = executor,
                FromAddress = eventValue.From?.ToBase58(),
                ToAddress = treasuryAddress,
                Amount = eventValue.Amount,
                Symbol = symbol,
                TreasuryRecordType = TreasuryRecordType.Deposit,
                CreateTime = context.BlockTime
            }, context);

            Logger.LogInformation(
                "[Transferred] FINISH: daoId={Id}, ChainId={ChainId}, treasuryAddress={treasuryAddress}", daoId,
                chainId, treasuryAddress);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Transferred] Exception to={treasuryAddress}, ChainId={ChainId}", treasuryAddress,
                chainId);
            throw;
        }
    }

    private async Task CreateOrUpdateTreasuryFundIndex(string chainId, string daoId, string symbol,
        Transferred eventValue, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(chainId, daoId, symbol);
        var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(id, chainId);
        if (treasuryFundIndex == null)
        {
            Logger.LogInformation(
                "[Transferred] TreasuryFund not support symbol: Id={Id}, ChainId={ChainId}, Symbol={Symbol}",
                daoId, chainId, symbol);
            treasuryFundIndex = new TreasuryFundIndex
            {
                Id = id,
                DaoId = daoId,
                TreasuryAddress = eventValue.To?.ToBase58(),
                Symbol = symbol
            };
        }

        treasuryFundIndex.AvailableFunds += eventValue.Amount;
        await SaveIndexAsync(treasuryFundIndex, context);
        Logger.LogInformation(
            "[Transferred] TreasuryFund FINISH: Id={Id}, ChainId={ChainId}, Symbol={Symbol}", daoId, chainId, symbol);
    }
}