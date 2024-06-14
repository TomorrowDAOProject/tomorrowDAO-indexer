using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using VoteMechanism = TomorrowDAO.Contracts.Vote.VoteMechanism;
using VoteOption = TomorrowDAO.Indexer.Plugin.Enums.VoteOption;

namespace TomorrowDAO.Indexer.Plugin.Processors.Vote;

public class VotedProcessor : VoteProcessorBase<Voted>
{
    public VotedProcessor(ILogger<AElfLogEventProcessorBase<Voted, LogEventInfo>> logger, IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<LatestParticipatedIndex, LogEventInfo> latestParticipatedRepository, 
        IVoteProvider voteProvider, IDAOProvider daoProvider)
        : base(logger, objectMapper, contractInfoOptions, latestParticipatedRepository, voteProvider, daoProvider)
    {
    }

    protected override async Task HandleEventAsync(Voted eventValue, LogEventContext context)
    {
        var voteId = eventValue.VoteId.ToHex();
        var chainId = context.ChainId;
        var voter = eventValue.Voter?.ToBase58();
        var DAOId = eventValue.DaoId?.ToHex();
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

            await UpdateDaoVoterInfoAsync(chainId, voteRecordIndex, context);

            if (eventValue.VoteMechanism == VoteMechanism.TokenBallot)
            {
                await UpdateDaoVoteAmountAsync(chainId, eventValue.DaoId.ToHex(), eventValue.Amount, context);
            }

            await SaveIndexAsync(new LatestParticipatedIndex
            {
                Id = IdGenerateHelper.GetId(chainId, voter),
                DAOId = DAOId, Address = voter,
                ParticipatedType = ParticipatedType.Voted,
                LatestParticipatedTime = context.BlockTime
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
            var daoVoterRecord = await _daoProvider.GetDaoVoterRecordAsync(chainId, id);
            if (daoVoterRecord == null)
            {
                daoVoterRecord = new DaoVoterRecordIndex
                {
                    Id = id,
                    DaoId = voteRecordIndex.DAOId,
                    VoterAddress = voteRecordIndex.Voter,
                    Count = 1,
                    ChainId = chainId
                };
                //update dao index
                var daoIndex = await _daoProvider.GetDAOAsync(chainId, voteRecordIndex.DAOId);
                if (daoIndex == null)
                {
                    Logger.LogError("[Voted] update DaoVoterRecord, Dao not found: daoId={Id}", voteRecordIndex.DAOId);
                }
                else
                {
                    daoIndex.VoterCount += 1;
                    await _daoProvider.SaveIndexAsync(daoIndex, context);
                    Logger.LogInformation("[Voted] update Dao Voter count: daoId={Id}, amount={amount}", voteRecordIndex.DAOId,
                        daoIndex.VoterCount);
                }
            }
            else
            {
                daoVoterRecord.Count += 1;
            }

            await _daoProvider.SaveDaoVoterRecordAsync(daoVoterRecord, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Voted] update DaoVoterRecord error: Id={Id}", id);
        }
    }
    
    private async Task UpdateDaoVoteAmountAsync(string chainId, string daoId, long amount, LogEventContext context)
    {
        try
        {
            Logger.LogInformation("[Voted] update DaoVoteAmount: daoId={Id}", daoId);
            var daoIndex = await _daoProvider.GetDAOAsync(chainId, daoId);
            if (daoIndex == null)
            {
                Logger.LogError("[Voted] update DaoVoteAmount error, Dao not found: daoId={Id}", daoId);
            }

            daoIndex!.VoteAmount = daoIndex.VoteAmount + amount;
            await _daoProvider.SaveIndexAsync(daoIndex, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[Voted] update DaoVoteAmount error: Id={Id}", daoId);
        }
    }

    private async Task UpdateVoteItemIndexAsync(string chainId, VoteRecordIndex voteRecordIndex,
        LogEventContext context)
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