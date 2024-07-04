using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.Vote;

public abstract class VoteProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public override string GetContractAddress(string chainId)
    {
        return chainId switch
        {
            TomorrowDAOConst.TestNetSideChainId => TomorrowDAOConst.VoteContractAddressTestNetSideChain,
            TomorrowDAOConst.MainNetSideChainId => TomorrowDAOConst.VoteContractAddressMainNetSideChain,
            _ => throw new Exception("Unknown chain id")
        };
    }
    
    protected async Task UpdateDaoVoteAmountAsync(string daoId, Action<DAOIndex> updateAction, LogEventContext context)
    {
        try
        {
            Logger.LogInformation("[VoteWithdrawn] update DaoVoteAmount: daoId={Id}", daoId);
            var daoIndex = await GetEntityAsync<DAOIndex>(daoId);
            if (daoIndex == null)
            {
                Logger.LogError("[VoteWithdrawn] update DaoVoteAmount error, Dao not found: daoId={Id}", daoId);
            }

            updateAction(daoIndex!);
            await SaveEntityAsync(daoIndex!, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VoteWithdrawn] update DaoVoteAmount error: Id={Id}", daoId);
        }
    }
    
    protected async Task UpdateVoteItemIndexAsync(string chainId, VoteRecordIndex voteRecordIndex,
        LogEventContext context)
    {
        Logger.LogInformation("[Voted] update VoteItemIndex: Id={Id}", voteRecordIndex.VotingItemId);
        var voteItemIndex = await GetEntityAsync<VoteItemIndex>(voteRecordIndex.VotingItemId);
        if (voteItemIndex == null)
        {
            Logger.LogError("[Voted] VoteItemIndex not found: VotingItemId={voteItemIndex}",
                voteRecordIndex.VotingItemId);
            return;
        }

        var amount = voteRecordIndex.VoteMechanism switch
        {
            TomorrowDAO.Indexer.Plugin.Enums.VoteMechanism.TOKEN_BALLOT => voteRecordIndex.Amount,
            TomorrowDAO.Indexer.Plugin.Enums.VoteMechanism.UNIQUE_VOTE => 1,
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
        var voterSet = voteItemIndex.VoterSet ?? new HashSet<string>() ;

        voterSet.Add(voteRecordIndex.VoteId);

        await SaveEntityAsync(voteItemIndex, context);
    }
}