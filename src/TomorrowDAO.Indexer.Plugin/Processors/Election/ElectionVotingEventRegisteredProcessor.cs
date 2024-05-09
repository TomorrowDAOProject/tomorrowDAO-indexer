using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Election;

public class ElectionVotingEventRegisteredProcessor : ElectionProcessorBase<ElectionVotingEventRegistered>
{
    public ElectionVotingEventRegisteredProcessor(
        ILogger<AElfLogEventProcessorBase<ElectionVotingEventRegistered, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger, objectMapper,
        contractInfoOptions, electionRepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(ElectionVotingEventRegistered eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[ElectionVotingEventRegistered] START: DaoId={Id}, ChainId={ChainId}", daoId, chainId);
        try
        {
            await HandleVotingItemIndexAsync(daoId, chainId, eventValue.VotingItem, context);

            await HandleHighCouncilConfigAsync(daoId, chainId, eventValue.Config, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[ElectionVotingEventRegistered] Exception Id={DAOId}, ChainId={ChainId}", daoId,
                chainId);
            throw;
        }
    }

    private async Task HandleVotingItemIndexAsync(string daoId, string chainId, VotingItem eventValue,
        LogEventContext context)
    {
        try
        {
            var votingItemId = IdGenerateHelper.GetId(daoId, chainId);

            var votingItemIndex = await _electionProvider.GetVotingItemIndexAsync(votingItemId, chainId);
            if (votingItemIndex != null)
            {
                Logger.LogInformation(
                    "[ElectionVotingItemIndex] candidate not existed: DaoId={Id}, ChainId={ChainId}", daoId,
                    chainId);
                return;
            }

            votingItemIndex = ObjectMapper.Map<VotingItem, ElectionVotingItemIndex>(eventValue);
            votingItemIndex.Id = votingItemId;
            votingItemIndex.DaoId = daoId;
            await _electionProvider.SaveVotingItemIndexAsync(votingItemIndex, context);
            Logger.LogInformation(
                "[ElectionVotingItemIndex] FINISH: Id={Id}, ChainId={ChainId}", votingItemId,
                chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[ElectionVotingItemIndex] Exception DaoId={DAOId}, ChainId={ChainId}, Candidate={candidate}", daoId,
                chainId);
            throw;
        }
    }

    private async Task HandleHighCouncilConfigAsync(string daoId, string chainId, HighCouncilConfig eventValue,
        LogEventContext context)
    {
        try
        {
            var highCouncilConfigId = IdGenerateHelper.GetId(daoId, chainId);

            var highCouncilConfigIndex =
                await _electionProvider.GetHighCouncilConfigIndexAsync(highCouncilConfigId, chainId);
            if (highCouncilConfigIndex != null)
            {
                Logger.LogInformation(
                    "[ElectionHighCouncilConfigIndex] candidate not existed: DaoId={Id}, ChainId={ChainId}", daoId,
                    chainId);
                return;
            }

            highCouncilConfigIndex = ObjectMapper.Map<HighCouncilConfig, ElectionHighCouncilConfigIndex>(eventValue);
            highCouncilConfigIndex.Id = highCouncilConfigId;
            highCouncilConfigIndex.DaoId = daoId;
            await _electionProvider.SaveHighCouncilConfigIndexAsync(highCouncilConfigIndex, context);
            Logger.LogInformation(
                "[ElectionHighCouncilConfigIndex] FINISH: Id={Id}, ChainId={ChainId}", highCouncilConfigId,
                chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[ElectionHighCouncilConfigIndex] Exception DaoId={DAOId}, ChainId={ChainId}, Candidate={candidate}",
                daoId,
                chainId);
            throw;
        }
    }
}