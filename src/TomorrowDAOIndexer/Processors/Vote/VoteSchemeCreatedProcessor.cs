using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VoteSchemeCreatedProcessor : VoteProcessorBase<VoteSchemeCreated>
{ 
    public override async Task ProcessAsync(VoteSchemeCreated logEvent, LogEventContext context)
    {
        var voteSchemeId = logEvent.VoteSchemeId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[VoteSchemeCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            voteSchemeId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var voteSchemeIndex = await GetEntityAsync<VoteSchemeIndex>(voteSchemeId);
            if (voteSchemeIndex != null)
            {
                Logger.LogInformation("[VoteSchemeCreated] VoteScheme already existed: Id={Id}, ChainId={ChainId}",
                    voteSchemeId, chainId);
                return;
            }

            voteSchemeIndex = ObjectMapper.Map<VoteSchemeCreated, VoteSchemeIndex>(logEvent);
            voteSchemeIndex.Id = voteSchemeId;
            voteSchemeIndex.CreateTime = context.Block.BlockTime;
            await SaveEntityAsync(voteSchemeIndex, context);
            Logger.LogInformation("[VoteSchemeCreated] FINISH: Id={Id}, ChainId={ChainId}", voteSchemeId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VoteSchemeCreated] Exception Id={Id}, ChainId={ChainId}", voteSchemeId, chainId);
            throw;
        }
    }
}