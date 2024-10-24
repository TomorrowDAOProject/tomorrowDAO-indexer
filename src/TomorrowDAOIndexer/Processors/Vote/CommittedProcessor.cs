using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.Vote;

public class CommittedProcessor : VoteProcessorBase<Committed>
{
    public override async Task ProcessAsync(Committed logEvent, LogEventContext context)
    {
        var transactionId = context.Transaction.TransactionId ?? string.Empty;
        var chainId = context.ChainId;
        var voter = context.Transaction.From;
        var daoId = logEvent.DaoId?.ToHex();
        Logger.LogInformation("[Committed] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            transactionId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var commitmentIndex = await GetEntityAsync<CommitmentIndex>(transactionId);
            if (commitmentIndex != null)
            {
                Logger.LogInformation("[Committed] already exists: Id={Id}, ChainId={ChainId}", transactionId,
                    chainId);
                return;
            }

            commitmentIndex = ObjectMapper.Map<Committed, CommitmentIndex>(logEvent);
            commitmentIndex.Id = transactionId;
            commitmentIndex.TransactionId = transactionId;
            commitmentIndex.Voter = voter;
            commitmentIndex.BlockHeight = context.Block.BlockHeight;
            await SaveEntityAsync(commitmentIndex, context);

            await SaveEntityAsync(new LatestParticipatedIndex
            {
                Id = IdGenerateHelper.GetId(chainId, daoId, voter),
                DAOId = daoId, Address = voter,
                ParticipatedType = ParticipatedType.Committed,
                LatestParticipatedTime = context.Block.BlockTime,
                NewData = true
            }, context);
            Logger.LogInformation("[Committed] FINISH: Id={Id}, ChainId={ChainId}", transactionId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Committed] Exception Id={Id}, ChainId={ChainId}", transactionId, chainId);
            throw;
        }
    }
}