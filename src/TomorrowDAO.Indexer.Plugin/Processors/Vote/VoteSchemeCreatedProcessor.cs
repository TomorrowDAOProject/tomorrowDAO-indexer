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

public class VoteSchemeCreatedProcessor : VoteProcessorBase<VoteSchemeCreated>
{
    public VoteSchemeCreatedProcessor(ILogger<AElfLogEventProcessorBase<VoteSchemeCreated, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IVoteProvider voteProvider)
        : base(logger, objectMapper, contractInfoOptions, voteProvider)
    {
    }

    protected override async Task HandleEventAsync(VoteSchemeCreated eventValue, LogEventContext context)
    {
        var voteSchemeId = eventValue.VoteSchemeId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[VoteSchemeCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            voteSchemeId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var voteSchemeIndex = await VoteProvider.GetVoteSchemeAsync(chainId, voteSchemeId);
            if (voteSchemeIndex != null)
            {
                Logger.LogInformation("[VoteSchemeCreated] VoteScheme already existed: Id={Id}, ChainId={ChainId}", voteSchemeId, chainId);
                return;
            }
            voteSchemeIndex = ObjectMapper.Map<VoteSchemeCreated, VoteSchemeIndex>(eventValue);
            voteSchemeIndex.Id = voteSchemeId;
            voteSchemeIndex.CreateTime = context.BlockTime;
            await VoteProvider.SaveVoteSchemeIndexAsync(voteSchemeIndex, context);
            Logger.LogInformation("[VoteSchemeCreated] FINISH: Id={Id}, ChainId={ChainId}", voteSchemeId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VoteSchemeCreated] Exception Id={Id}, ChainId={ChainId}", voteSchemeId, chainId);
            throw;
        }
    }
}