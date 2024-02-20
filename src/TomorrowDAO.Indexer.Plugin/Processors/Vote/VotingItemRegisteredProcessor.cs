using AElfIndexer.Client.Handlers;
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
    public VotingItemRegisteredProcessor(ILogger<VoteProcessorBase<VotingItemRegistered>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IVoteProvider voteProvider) 
        : base(logger, objectMapper, contractInfoOptions, voteProvider)
    {
    }

    protected override async Task HandleEventAsync(VotingItemRegistered eventValue, LogEventContext context)
    {
        var votingItemId = eventValue.VotingItemId.ToHex();
        var DAOId = eventValue.DaoId.ToHex();
        var voteSchemeId = eventValue.SchemeId.ToHex();
        var chainId = context.ChainId;
        var id = IdGenerateHelper.GetId(chainId, votingItemId, DAOId, voteSchemeId);
        Logger.LogInformation("[VotingItemRegistered] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            id, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var voteItemIndex = await VoteProvider.GetVoteItemAsync(chainId, id);
            if (voteItemIndex != null)
            {
                Logger.LogInformation("[VotingItemRegistered] VoteItem already existed: Id={Id}, ChainId={ChainId}", id, chainId);
                return;
            }
            voteItemIndex = ObjectMapper.Map<VotingItemRegistered, VoteIndex>(eventValue);
            voteItemIndex.Id = id;
            voteItemIndex.CreateTime = context.BlockTime;
            await VoteProvider.SaveVoteItemIndexAsync(voteItemIndex, context);
            Logger.LogInformation("[VotingItemRegistered] FINISH: Id={Id}, ChainId={ChainId}", id, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VotingItemRegistered] Exception Id={Id}, ChainId={ChainId}", id, chainId);
            throw;
        }
    }
}