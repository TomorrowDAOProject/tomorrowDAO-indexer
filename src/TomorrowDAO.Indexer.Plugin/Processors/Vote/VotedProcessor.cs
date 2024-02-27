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
                Logger.LogInformation("[Voted] VoteRecord already existed: Id={Id}, ChainId={ChainId}", voteId, chainId);
                return;
            }
            voteRecordIndex = ObjectMapper.Map<Voted, VoteRecordIndex>(eventValue);
            voteRecordIndex.Id = voteId;
            await VoteProvider.SaveVoteRecordIndexAsync(voteRecordIndex, context);
            Logger.LogInformation("[Voted] FINISH: Id={Id}, ChainId={ChainId}", voteId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Voted] Exception Id={Id}, ChainId={ChainId}", voteId, chainId);
            throw;
        }
    }
}