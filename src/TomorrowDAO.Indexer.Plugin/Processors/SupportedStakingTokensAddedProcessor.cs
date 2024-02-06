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

public class SupportedStakingTokensAddedProcessor : TreasuryProcessorBase<SupportedStakingTokensAdded>
{
    public SupportedStakingTokensAddedProcessor(ILogger<DAOProcessorBase<SupportedStakingTokensAdded>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<TreasuryFundIndex, LogEventInfo> treasuryFundRepository,
        IAElfIndexerClientEntityRepository<TreasuryRecordIndex, LogEventInfo> treasuryRecordRepository, IDAOProvider DAOProvider) 
        : base(logger, objectMapper, contractInfoOptions, treasuryFundRepository, treasuryRecordRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(SupportedStakingTokensAdded eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[SupportedStakingTokensAdded] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            await AddSymbolList(eventValue.AddedTokens.Data.ToList(), DAOId, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[SupportedStakingTokensAdded] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}