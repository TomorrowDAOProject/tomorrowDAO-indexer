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

public class VoteWithdrawnProcessor : VoteProcessorBase<Withdrawn>
{
    public VoteWithdrawnProcessor(ILogger<AElfLogEventProcessorBase<Withdrawn, LogEventInfo>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IVoteProvider voteProvider, IDAOProvider daoProvider) : base(logger, objectMapper, contractInfoOptions,
        voteProvider, daoProvider)
    {
    }

    protected override async Task HandleEventAsync(Withdrawn eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var id = IdGenerateHelper.GetId(chainId, daoId, context.TransactionId);
        Logger.LogInformation("[VoteWithdrawn] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            id, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var withdrawnIndex = await VoteProvider.GetVoteWithdrawnAsync(chainId, id);
            if (withdrawnIndex != null)
            {
                Logger.LogInformation(
                    "[VoteWithdrawn] VoteWithdrawnIndexDto already existed: Id={Id}, ChainId={ChainId}",
                    id, chainId);
                return;
            }

            withdrawnIndex = ObjectMapper.Map<Withdrawn, VoteWithdrawnIndex>(eventValue);
            withdrawnIndex.Id = id;
            withdrawnIndex.CreateTime = context.BlockTime;
            await VoteProvider.SaveVoteWithdrawnAsync(withdrawnIndex, context);
            
            await UpdateDaoVoteAmountAsync(chainId, eventValue.DaoId.ToHex(), eventValue.WithdrawAmount, context);

            
            Logger.LogInformation("[VoteWithdrawn] FINISH: Id={Id}, ChainId={ChainId}", id, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VoteWithdrawn] Exception Id={Id}, ChainId={ChainId}", id, chainId);
            throw;
        }
    }

    private async Task UpdateDaoVoteAmountAsync(string chainId, string daoId, long amount, LogEventContext context)
    {
        try
        {
            Logger.LogInformation("[VoteWithdrawn] update DaoVoteAmount: daoId={Id}", daoId);
            var daoIndex = await _daoProvider.GetDaoAsync(chainId, daoId);
            if (daoIndex == null)
            {
                Logger.LogError("[VoteWithdrawn] update DaoVoteAmount error, Dao not found: daoId={Id}", daoId);
            }

            daoIndex!.WithdrawAmount = daoIndex.WithdrawAmount + amount;
            await _daoProvider.SaveIndexAsync(daoIndex, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VoteWithdrawn] update DaoVoteAmount error: Id={Id}", daoId);
        }
    }
}