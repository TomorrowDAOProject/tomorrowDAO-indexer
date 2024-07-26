using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VotingItemRegisteredProcessor : VoteProcessorBase<VotingItemRegistered>
{
    public override async Task ProcessAsync(VotingItemRegistered logEvent, LogEventContext context)
    {
        var votingItemId = logEvent.VotingItemId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[VotingItemRegistered] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            votingItemId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var voteItemIndex = await GetEntityAsync<VoteItemIndex>(votingItemId);
            if (voteItemIndex != null)
            {
                Logger.LogInformation("[VotingItemRegistered] VoteItem already existed: Id={Id}, ChainId={ChainId}",
                    votingItemId, chainId);
                return;
            }

            voteItemIndex = ObjectMapper.Map<VotingItemRegistered, VoteItemIndex>(logEvent);
            voteItemIndex.Id = votingItemId;
            voteItemIndex.CreateTime = context.Block.BlockTime;
            await SaveEntityAsync(voteItemIndex, context);
            Logger.LogInformation("[VotingItemRegistered] FINISH: Id={Id}, ChainId={ChainId}", votingItemId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VotingItemRegistered] Exception Id={Id}, ChainId={ChainId}", votingItemId, chainId);
            throw;
        }
    }
}