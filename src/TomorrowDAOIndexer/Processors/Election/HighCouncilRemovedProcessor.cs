using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.Election;

public class HighCouncilRemovedProcessor : ElectionProcessorBase<HighCouncilRemoved>
{
    public override async Task ProcessAsync(HighCouncilRemoved logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilRemoved] START: DaoId={Id}, ChainId={ChainId}", daoId, chainId);
        try
        {
            // var addressList = new List<string>();
            // foreach (var address in logEvent.RemoveHighCouncils.Value)
            // {
            //     addressList.Add(address.ToBase58());
            // }
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
            var councilMembers = new List<string>();
            foreach (var member in highCouncilConfigIndex.InitialHighCouncilMembers)
            {
                if (!addressList.Contains(member))
                {
                    councilMembers.Add(member);
                }
            }
            highCouncilConfigIndex.InitialHighCouncilMembers = councilMembers;
            // highCouncilConfigIndex.InitialHighCouncilMembers.RemoveAll(item => addressList.Contains(item));
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