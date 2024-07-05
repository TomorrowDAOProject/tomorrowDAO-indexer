using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class VotedProcessor : ElectionProcessorBase<Voted>
{
    public override async Task ProcessAsync(Voted logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = logEvent.CandidateAddress.ToBase58();
        Logger.LogInformation("[ElectionVoted] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            daoId, chainId, candidate);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, daoId, candidate, CandidateTerm);
            var electionIndex = await GetEntityAsync<ElectionIndex>(id);
            if (electionIndex == null)
            {
                Logger.LogError("[ElectionVoted] ElectionIndex not exist: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
                    daoId, chainId, candidate);
                electionIndex = new ElectionIndex
                {
                    Address = candidate,
                    DAOId = daoId,
                    TermNumber = CandidateTerm,
                    HighCouncilType = HighCouncilType.Candidate,
                    VotesAmount = logEvent.Amount,
                    Id = id
                };
            }
            else
            {
                electionIndex.VotesAmount += logEvent.Amount;
            }
            
            await SaveEntityAsync(electionIndex, context);

            Logger.LogInformation("[ElectionVoted] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId,
                chainId, candidate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ElectionVoted] Exception Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId,
                chainId, candidate);
            throw;
        }
    }
}