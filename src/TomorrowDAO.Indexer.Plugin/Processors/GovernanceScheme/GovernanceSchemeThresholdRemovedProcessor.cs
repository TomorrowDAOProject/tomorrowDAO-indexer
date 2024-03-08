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

public class GovernanceSchemeThresholdRemovedProcessor : GovernanceSchemeProcessorBase<GovernanceSchemeThresholdRemoved>
{
    public GovernanceSchemeThresholdRemovedProcessor(
        ILogger<AElfLogEventProcessorBase<GovernanceSchemeThresholdRemoved, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
        IAElfIndexerClientEntityRepository<GovernanceMechanismIndex, LogEventInfo> governanceMechanismRepository,
        IDAOProvider DAOProvider) :
        base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, governanceMechanismRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(GovernanceSchemeThresholdRemoved eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var schemeAddress = eventValue.SchemeAddress?.ToBase58();
        var DAOId = eventValue.DaoId?.ToHex();
        var id = IdGenerateHelper.GetId(chainId, DAOId, schemeAddress);
        Logger.LogInformation("[GovernanceSchemeThresholdRemoved] start id {id}", id);
        try
        {
            var governanceSchemeIndex = await GovernanceSchemeRepository.GetFromBlockStateSetAsync(id, chainId);
            if (governanceSchemeIndex == null)
            {
                Logger.LogInformation("[GovernanceSchemeThresholdRemoved] GovernanceScheme not existed id {id}", id);
                return;
            }
            await GovernanceSchemeRepository.DeleteAsync(governanceSchemeIndex);
            Logger.LogInformation("[GovernanceSchemeThresholdRemoved] end id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[GovernanceSchemeThresholdRemoved] Exception Id={id}", id);
            throw;
        }
    }
}