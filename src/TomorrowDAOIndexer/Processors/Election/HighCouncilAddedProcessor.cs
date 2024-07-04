using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class HighCouncilAddedProcessor : ElectionProcessorBase<HighCouncilAdded>, ISingletonDependency
{
    public override async Task ProcessAsync(HighCouncilAdded logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilAdded] START: DaoId={Id}, ChainId={ChainId}", daoId, chainId);
        try
        {
            var addressList = logEvent.AddHighCouncils.Value.Select(address => address.ToBase58()).ToList();

            var highCouncilConfigId = IdGenerateHelper.GetId(daoId, chainId);

            var highCouncilConfigIndex = await GetEntityAsync<ElectionHighCouncilConfigIndex>(highCouncilConfigId);
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

            await SaveEntityAsync(highCouncilConfigIndex, context);
            Logger.LogInformation(
                "[HighCouncilAdded] Update ElectionHighCouncilConfigIndex FINISH: Id={Id}, ChainId={ChainId}",
                highCouncilConfigId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[HighCouncilAdded] Exception Id={DAOId}, ChainId={ChainId}",
                daoId, chainId);
            throw;
        }
    }
}