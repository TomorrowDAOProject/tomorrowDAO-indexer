using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

public class GovernanceTokenSetProcessor : GovernanceSchemeProcessorBase<GovernanceTokenSet>
{
    public GovernanceTokenSetProcessor(
        ILogger<AElfLogEventProcessorBase<GovernanceTokenSet, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
        IDAOProvider DAOProvider) :
        base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(GovernanceTokenSet eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var DAOId = eventValue.DaoId?.ToHex();
        var governanceToken = eventValue.GovernanceToken;
        Logger.LogInformation("[GovernanceTokenSet] start DAOId {DAOId} governanceToken {governanceToken}", DAOId, governanceToken);
        try
        {
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            if (DAOIndex != null)
            {
                DAOIndex.GovernanceToken = governanceToken;
                await DAOProvider.SaveIndexAsync(DAOIndex, context);
            }
            
            Logger.LogInformation("[GovernanceTokenSet] end DAOId {DAOId} governanceToken {governanceToken}", DAOId, governanceToken);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[GovernanceTokenSet] Exception DAOId {DAOId} governanceToken {governanceToken}", DAOId, governanceToken);
            throw;
        }
    }
}