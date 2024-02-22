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

public class SupportedStakingTokensRemovedProcessor : TreasuryProcessorBase<SupportedStakingTokensRemoved>
{
    public SupportedStakingTokensRemovedProcessor(ILogger<DAOProcessorBase<SupportedStakingTokensRemoved>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository, IDAOProvider DAOProvider) 
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(SupportedStakingTokensRemoved eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[SupportedStakingTokensRemoved] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var symbolList = eventValue.RemovedTokens.Data.ToList();
            foreach (var id in symbolList.Select(symbol => IdGenerateHelper.GetId(chainId, DAOId, symbol)))
            {
                var treasuryFundIndex = await TreasuryFundRepository.GetFromBlockStateSetAsync(id, chainId);
                if (treasuryFundIndex != null)
                {
                    await TreasuryFundRepository.DeleteAsync(treasuryFundIndex);
                }
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[SupportedStakingTokensRemoved] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}