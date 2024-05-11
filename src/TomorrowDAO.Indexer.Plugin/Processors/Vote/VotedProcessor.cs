using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using VoteOption = TomorrowDAO.Indexer.Plugin.Enums.VoteOption;

namespace TomorrowDAO.Indexer.Plugin.Processors.Vote;

public class VotedProcessor : VoteProcessorBase<Voted>
{
    public VotedProcessor(ILogger<AElfLogEventProcessorBase<Voted, LogEventInfo>> logger, IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IVoteProvider voteProvider)
        : base(logger, objectMapper, contractInfoOptions, voteProvider)
    {
    }

    protected override async Task HandleEventAsync(Voted eventValue, LogEventContext context)
    {
        var voteId = eventValue.VoteId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[Voted] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            voteId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var voteRecordIndex = await VoteProvider.GetVoteRecordAsync(chainId, voteId);
            if (voteRecordIndex != null)
            {
                Logger.LogInformation("[Voted] VoteRecord already existed: Id={Id}, ChainId={ChainId}", voteId,
                    chainId);
                return;
            }

            voteRecordIndex = ObjectMapper.Map<Voted, VoteRecordIndex>(eventValue);
            voteRecordIndex.Id = voteId;
            await VoteProvider.SaveVoteRecordIndexAsync(voteRecordIndex, context);
            
            await UpdateVoteItemIndexAsync(chainId, voteRecordIndex, context);
            
            Logger.LogInformation("[Voted] FINISH: Id={Id}, ChainId={ChainId}", voteId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Voted] Exception Id={Id}, ChainId={ChainId}", voteId, chainId);
            throw;
        }
    }

    private async Task UpdateVoteItemIndexAsync(string chainId, VoteRecordIndex voteRecordIndex, LogEventContext context)
    {
        Logger.LogInformation("[Voted] update VoteItemIndex: Id={Id}", voteRecordIndex.VotingItemId);
        var voteItemIndex = await VoteProvider.GetVoteItemAsync(chainId, voteRecordIndex.VotingItemId);
        if (voteItemIndex == null)
        {
            Logger.LogError("[Voted] VoteItemIndex not found: VotingItemId={voteItemIndex}",
                voteRecordIndex.VotingItemId);
            return;
        }

        var amount = voteRecordIndex.VoteMechanism switch
        {
            Enums.VoteMechanism.TOKEN_BALLOT => voteRecordIndex.Amount,
            Enums.VoteMechanism.UNIQUE_VOTE => 1,
            _ => 0
        };

        var voteOption = voteRecordIndex.Option;
        switch (voteOption)
        {
            case VoteOption.Approved:
                voteItemIndex.ApprovedCount += amount;
                break;
            case VoteOption.Rejected:
                voteItemIndex.RejectionCount += amount;
                break;
            case VoteOption.Abstained:
                voteItemIndex.AbstentionCount += amount;
                break;
        }

        voteItemIndex.VotesAmount += amount;
        voteItemIndex.VoterCount += 1;
        var voterSet = voteItemIndex.VoterSet;
        if (voterSet == null)
        {
            voterSet = new HashSet<string>();
            voteItemIndex.VoterSet = voterSet;
        }

        voterSet.Add(voteRecordIndex.VoteId);

        await VoteProvider.SaveVoteItemIndexAsync(voteItemIndex, context);
    }
}