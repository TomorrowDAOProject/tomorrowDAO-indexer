using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Election;

public class HighCouncilRemovedProcessor : ElectionProcessorBase<HighCouncilRemoved>
{
    public HighCouncilRemovedProcessor(
        ILogger<AElfLogEventProcessorBase<HighCouncilRemoved, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger, objectMapper,
        contractInfoOptions, electionRepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilRemoved eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilRemoved] START: DaoId={Id}, ChainId={ChainId}", daoId, chainId);
        try
        {
            var addressList = eventValue.RemoveHighCouncils.Value.Select(address => address.ToBase58()).ToList();

            var highCouncilConfigId = IdGenerateHelper.GetId(daoId, chainId);

            var highCouncilConfigIndex =
                await ElectionProvider.GetHighCouncilConfigIndexAsync(highCouncilConfigId, chainId);
            if (highCouncilConfigIndex == null)
            {
                Logger.LogError(
                    "[HighCouncilRemoved] ElectionHighCouncilConfigIndex not existed: DaoId={Id}, ChainId={ChainId}",
                    daoId, chainId);
                return;
            }
            Logger.LogInformation(
                "[HighCouncilRemoved] ElectionHighCouncilConfigIndex existed: DaoId={Id}, ChainId={ChainId}",
                daoId, chainId);
            highCouncilConfigIndex.InitialHighCouncilMembers.RemoveAll(item => addressList.Contains(item));

            await ElectionProvider.SaveHighCouncilConfigIndexAsync(highCouncilConfigIndex, context);
            Logger.LogInformation(
                "[HighCouncilRemoved] Update ElectionHighCouncilConfigIndex FINISH: Id={Id}, ChainId={ChainId}",
                highCouncilConfigId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[HighCouncilRemoved] Exception Id={DAOId}, ChainId={ChainId}", daoId,
                chainId);
            throw;
        }
    }
}