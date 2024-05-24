using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

public class GovernanceSchemeAddedProcessor : GovernanceSchemeProcessorBase<GovernanceSchemeAdded>
{
    public GovernanceSchemeAddedProcessor(
        ILogger<AElfLogEventProcessorBase<GovernanceSchemeAdded, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
        IDAOProvider DAOProvider) :
        base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(GovernanceSchemeAdded eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var schemeAddress = eventValue.SchemeAddress?.ToBase58();
        var DAOId = eventValue.DaoId?.ToHex();
        var id = IdGenerateHelper.GetId(chainId, DAOId, schemeAddress);
        Logger.LogInformation("[GovernanceSchemeAdded] start id {id}", id);
        try
        {
            var governanceSchemeIndex = await GovernanceSchemeRepository.GetFromBlockStateSetAsync(id, chainId);
            if (governanceSchemeIndex != null)
            {
                Logger.LogInformation("[GovernanceSchemeAdded] GovernanceScheme already existed id {id}", id);
                return;
            }
            governanceSchemeIndex = ObjectMapper.Map<GovernanceSchemeAdded, GovernanceSchemeIndex>(eventValue);
            governanceSchemeIndex.Id = id;
            governanceSchemeIndex.CreateTime = context.BlockTime;
            governanceSchemeIndex.OfThreshold(eventValue.SchemeThreshold);
            governanceSchemeIndex.ChainId = chainId;
            await SaveIndexAsync(governanceSchemeIndex, context);
            Logger.LogInformation("[GovernanceSchemeAdded] end id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[GovernanceSchemeAdded] Exception Id={id}", id);
            throw;
        }
    }
}