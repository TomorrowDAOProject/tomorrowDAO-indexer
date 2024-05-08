using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Vote;

public class VotingItemRegisteredProcessor : VoteProcessorBase<VotingItemRegistered>
{
    public VotingItemRegisteredProcessor(ILogger<AElfLogEventProcessorBase<VotingItemRegistered, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IVoteProvider voteProvider)
        : base(logger, objectMapper, contractInfoOptions, voteProvider)
    {
    }

    protected override async Task HandleEventAsync(VotingItemRegistered eventValue, LogEventContext context)
    {
        var votingItemId = eventValue.VotingItemId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[VotingItemRegistered] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            votingItemId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var voteItemIndex = await VoteProvider.GetVoteItemAsync(chainId, votingItemId);
            if (voteItemIndex != null)
            {
                Logger.LogInformation("[VotingItemRegistered] VoteItem already existed: Id={Id}, ChainId={ChainId}", votingItemId, chainId);
                return;
            }
            voteItemIndex = ObjectMapper.Map<VotingItemRegistered, VoteItemIndex>(eventValue);
            voteItemIndex.Id = votingItemId;
            voteItemIndex.CreateTime = context.BlockTime;
            await VoteProvider.SaveVoteItemIndexAsync(voteItemIndex, context);
            Logger.LogInformation("[VotingItemRegistered] FINISH: Id={Id}, ChainId={ChainId}", votingItemId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VotingItemRegistered] Exception Id={Id}, ChainId={ChainId}", votingItemId, chainId);
            throw;
        }
    }
}