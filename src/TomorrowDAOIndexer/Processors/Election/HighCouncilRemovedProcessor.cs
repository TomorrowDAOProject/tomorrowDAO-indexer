using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class HighCouncilRemovedProcessor : ElectionProcessorBase<HighCouncilRemoved>, ISingletonDependency
{
    public override async Task ProcessAsync(HighCouncilRemoved logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilRemoved] START: DaoId={Id}, ChainId={ChainId}", daoId, chainId);
        try
        {
            var addressList = logEvent.RemoveHighCouncils.Value.Select(address => address.ToBase58()).ToList();

            var highCouncilConfigId = IdGenerateHelper.GetId(daoId, chainId);

            var highCouncilConfigIndex = await GetEntityAsync<ElectionHighCouncilConfigIndex>(highCouncilConfigId);
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

            await SaveEntityAsync(highCouncilConfigIndex, context);
            Logger.LogInformation(
                "[HighCouncilRemoved] Update ElectionHighCouncilConfigIndex FINISH: Id={Id}, ChainId={ChainId}",
                highCouncilConfigId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[HighCouncilRemoved] Exception Id={DAOId}, ChainId={ChainId}",
                daoId, chainId);
            throw;
        }
    }
}