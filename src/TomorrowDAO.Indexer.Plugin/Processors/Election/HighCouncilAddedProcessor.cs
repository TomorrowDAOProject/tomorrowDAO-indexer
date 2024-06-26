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

public class HighCouncilAddedProcessor : ElectionProcessorBase<HighCouncilAdded>
{
    public HighCouncilAddedProcessor(
        ILogger<AElfLogEventProcessorBase<HighCouncilAdded, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger, objectMapper,
        contractInfoOptions, electionRepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilAdded eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilAdded] START: DaoId={Id}, ChainId={ChainId}", daoId, chainId);
        try
        {
            var addressList = eventValue.AddHighCouncils.Value.Select(address => address.ToBase58()).ToList();

            var highCouncilConfigId = IdGenerateHelper.GetId(daoId, chainId);

            var highCouncilConfigIndex =
                await ElectionProvider.GetHighCouncilConfigIndexAsync(highCouncilConfigId, chainId);
            if (highCouncilConfigIndex == null)
            {
                Logger.LogInformation(
                    "[HighCouncilAdded] ElectionHighCouncilConfigIndex not existed: DaoId={Id}, ChainId={ChainId}",
                    daoId, chainId);
                highCouncilConfigIndex = new ElectionHighCouncilConfigIndex
                {
                    Id = highCouncilConfigId,
                    DaoId = daoId,
                    InitialHighCouncilMembers = addressList
                };
            }
            else
            {
                Logger.LogInformation(
                    "[HighCouncilAdded] ElectionHighCouncilConfigIndex existed: DaoId={Id}, ChainId={ChainId}",
                    daoId, chainId);
                highCouncilConfigIndex.InitialHighCouncilMembers.AddRange(addressList);
            }

            await ElectionProvider.SaveHighCouncilConfigIndexAsync(highCouncilConfigIndex, context);
            Logger.LogInformation(
                "[HighCouncilAdded] Update ElectionHighCouncilConfigIndex FINISH: Id={Id}, ChainId={ChainId}",
                highCouncilConfigId,
                chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[HighCouncilAdded] Exception Id={DAOId}, ChainId={ChainId}", daoId,
                chainId);
            throw;
        }
    }
}