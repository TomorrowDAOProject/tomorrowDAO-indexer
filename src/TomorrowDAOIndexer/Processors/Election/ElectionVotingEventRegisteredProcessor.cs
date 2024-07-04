using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class ElectionVotingEventRegisteredProcessor : ElectionProcessorBase<ElectionVotingEventRegistered>, ISingletonDependency
{
    public override async Task ProcessAsync(ElectionVotingEventRegistered logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[ElectionVotingEventRegistered] START: DaoId={Id}, ChainId={ChainId}", daoId, chainId);
        try
        {
            await HandleVotingItemIndexAsync(daoId, chainId, logEvent.VotingItem, context);

            await HandleHighCouncilConfigAsync(daoId, chainId, logEvent.Config, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[ElectionVotingEventRegistered] Exception Id={DAOId}, ChainId={ChainId}", daoId,
                chainId);
            throw;
        }
    }

    private async Task HandleVotingItemIndexAsync(string daoId, string chainId, VotingItem votingItem,
        LogEventContext context)
    {
        try
        {
            Logger.LogInformation(
                "[ElectionVotingEventRegistered] Update ElectionVotingItemIndex: DaoId={Id}, ChainId={ChainId}",
                daoId, chainId);
            var votingItemId = IdGenerateHelper.GetId(daoId, chainId);

            
            var votingItemIndex = await GetEntityAsync<ElectionVotingItemIndex>(votingItemId);;
            if (votingItemIndex != null)
            {
                Logger.LogInformation(
                    "[ElectionVotingEventRegistered] ElectionVotingItemIndex existed: DaoId={Id}, ChainId={ChainId}",
                    daoId, chainId);
                return;
            }

            votingItemIndex = ObjectMapper.Map<VotingItem, ElectionVotingItemIndex>(votingItem);
            votingItemIndex.Id = votingItemId;
            votingItemIndex.DaoId = daoId;
            
            await SaveEntityAsync(votingItemIndex, context);
            Logger.LogInformation(
                "[ElectionVotingEventRegistered] Update ElectionVotingItemIndex FINISH: Id={Id}, ChainId={ChainId}",
                votingItemId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[ElectionVotingEventRegistered] Update ElectionVotingItemIndex error: DaoId={DAOId}, ChainId={ChainId}, VotingItem={VotingItem}",
                daoId, chainId, JsonConvert.SerializeObject(votingItem));
            throw;
        }
    }

    private async Task HandleHighCouncilConfigAsync(string daoId, string chainId, HighCouncilConfig highCouncilConfig,
        LogEventContext context)
    {
        try
        {
            Logger.LogInformation(
                "[ElectionVotingEventRegistered] Update ElectionHighCouncilConfigIndex: DaoId={Id}, ChainId={ChainId}",
                daoId,
                chainId);
            var highCouncilConfigId = IdGenerateHelper.GetId(daoId, chainId);

            var highCouncilConfigIndex = await GetEntityAsync<ElectionHighCouncilConfigIndex>(highCouncilConfigId);
            if (highCouncilConfigIndex == null)
            {
                Logger.LogInformation(
                    "[ElectionVotingEventRegistered] ElectionHighCouncilConfigIndex not existed: DaoId={Id}, ChainId={ChainId}",
                    daoId,
                    chainId);
                highCouncilConfigIndex =
                    ObjectMapper.Map<HighCouncilConfig, ElectionHighCouncilConfigIndex>(highCouncilConfig);
                highCouncilConfigIndex.Id = highCouncilConfigId;
                highCouncilConfigIndex.DaoId = daoId;
            }
            else
            {
                Logger.LogInformation(
                    "[ElectionVotingEventRegistered] ElectionHighCouncilConfigIndex existed: DaoId={Id}, ChainId={ChainId}",
                    daoId,
                    chainId);
                ObjectMapper.Map(highCouncilConfig, highCouncilConfigIndex);
            }

            await SaveEntityAsync(highCouncilConfigIndex, context);
            Logger.LogInformation(
                "[ElectionVotingEventRegistered] Update ElectionHighCouncilConfigIndex FINISH: Id={Id}, ChainId={ChainId}",
                highCouncilConfigId,
                chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[ElectionVotingEventRegistered] Update ElectionHighCouncilConfigIndex error: DaoId={DAOId}, ChainId={ChainId}, HighCouncilConfig={Config}",
                daoId, chainId, JsonConvert.SerializeObject(highCouncilConfig));
            throw;
        }
    }
}