using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VoteWithdrawnProcessor : VoteProcessorBase<Withdrawn>
{
    public override async Task ProcessAsync(Withdrawn logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var id = IdGenerateHelper.GetId(chainId, daoId, context.Transaction.TransactionId);
        Logger.LogInformation("[VoteWithdrawn] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            id, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var withdrawnIndex = await GetEntityAsync<VoteWithdrawnIndex>(id);
            if (withdrawnIndex != null)
            {
                Logger.LogInformation(
                    "[VoteWithdrawn] VoteWithdrawnIndexDto already existed: Id={Id}, ChainId={ChainId}",
                    id, chainId);
                return;
            }

            withdrawnIndex = ObjectMapper.Map<Withdrawn, VoteWithdrawnIndex>(logEvent);
            withdrawnIndex.Id = id;
            withdrawnIndex.CreateTime = context.Block.BlockTime;
            await SaveEntityAsync(withdrawnIndex, context);
            
            await UpdateDaoVoteAmountAsync(logEvent.DaoId.ToHex(), true, logEvent.WithdrawAmount, context);
            Logger.LogInformation("[VoteWithdrawn] FINISH: Id={Id}, ChainId={ChainId}", id, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VoteWithdrawn] Exception Id={Id}, ChainId={ChainId}", id, chainId);
            throw;
        }
    }
}