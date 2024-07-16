using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using VoteMechanism = TomorrowDAO.Contracts.Vote.VoteMechanism;

namespace TomorrowDAOIndexer.Processors.Vote;

public class VotedProcessor : VoteProcessorBase<Voted>
{
    public override async Task ProcessAsync(Voted logEvent, LogEventContext context)
    {
        var voteId = logEvent.VoteId?.ToHex() ?? string.Empty;
        var chainId = context.ChainId;
        var voter = logEvent.Voter?.ToBase58();
        var DAOId = logEvent.DaoId?.ToHex();
        Logger.LogInformation("[Voted] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            voteId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            if (voteId.IsNullOrWhiteSpace())
            {
                Logger.LogError(
                    "[Voted] VoteId is null: Id={Id}, ChainId={ChainId}, TransactionId={TransactionId}, Event={Event}",
                    voteId, chainId, context.Transaction.TransactionId, JsonConvert.SerializeObject(logEvent));
                return;
            }

            var voteRecordIndex = await GetEntityAsync<VoteRecordIndex>(voteId);
            if (voteRecordIndex != null)
            {
                Logger.LogInformation("[Voted] VoteRecord already existed: Id={Id}, ChainId={ChainId}", voteId,
                    chainId);
                return;
            }

            voteRecordIndex = ObjectMapper.Map<Voted, VoteRecordIndex>(logEvent);
            voteRecordIndex.Id = voteId!;
            await SaveEntityAsync(voteRecordIndex, context);

            await UpdateVoteItemIndexAsync(chainId, voteRecordIndex, context);

            await UpdateDaoVoterInfoAsync(chainId, voteRecordIndex, context);

            if (logEvent.VoteMechanism == VoteMechanism.TokenBallot)
            {
                await UpdateDaoVoteAmountAsync(DAOId, dao => dao.VoteAmount += logEvent.Amount, context);
            }

            await SaveEntityAsync(new LatestParticipatedIndex
            {
                Id = IdGenerateHelper.GetId(chainId, voter),
                DAOId = DAOId, Address = voter,
                ParticipatedType = ParticipatedType.Voted,
                LatestParticipatedTime = context.Block.BlockTime
            }, context);
            Logger.LogInformation("[Voted] FINISH: Id={Id}, ChainId={ChainId}", voteId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Voted] Exception Id={Id}, ChainId={ChainId}", voteId, chainId);
            throw;
        }
    }

    private async Task UpdateDaoVoterInfoAsync(string chainId, VoteRecordIndex voteRecordIndex, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(chainId, voteRecordIndex.DAOId, voteRecordIndex.Voter);
        try
        {
            Logger.LogInformation("[Voted] update DaoVoterRecord: Id={Id}", id);
            var daoVoterRecord = await GetEntityAsync<DaoVoterRecordIndex>(id);
            if (daoVoterRecord == null)
            {
                daoVoterRecord = new DaoVoterRecordIndex
                {
                    Id = id,
                    DaoId = voteRecordIndex.DAOId,
                    VoterAddress = voteRecordIndex.Voter,
                    Count = 1
                };
                //update dao index
                var daoIndex = await GetEntityAsync<DAOIndex>(voteRecordIndex.DAOId);
                if (daoIndex == null)
                {
                    Logger.LogError("[Voted] update DaoVoterRecord, Dao not found: daoId={Id}", voteRecordIndex.DAOId);
                }
                else
                {
                    daoIndex.VoterCount += 1;
                    await SaveEntityAsync(daoIndex, context);
                    Logger.LogInformation("[Voted] update Dao Voter count: daoId={Id}, amount={amount}",
                        voteRecordIndex.DAOId, daoIndex.VoterCount);
                }
            }
            else
            {
                daoVoterRecord.Count += 1;
            }

            await SaveEntityAsync(daoVoterRecord, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Voted] update DaoVoterRecord error: Id={Id}", id);
        }
    }
}