using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateRemovedProcessor : ElectionProcessorBase<CandidateRemoved>, ISingletonDependency
{
    public override async Task ProcessAsync(CandidateRemoved logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = logEvent.Candidate?.ToBase58();
        Logger.LogInformation("[CandidateRemoved] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            DAOId, chainId, candidate);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, DAOId, candidate, CandidateTerm);
            var electionIndex = await GetEntityAsync<ElectionIndex>(id);
            if (electionIndex == null)
            {
                Logger.LogInformation(
                    "[CandidateRemoved] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
                    DAOId, chainId, candidate);
                return;
            }

            await SaveEntityAsync(electionIndex);
            Logger.LogInformation("[CandidateRemoved] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", DAOId,
                chainId, candidate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[CandidateRemoved] Exception Id={DAOId}, ChainId={ChainId}, Candidate={candidate}",
                DAOId, chainId, candidate);
            throw;
        }
    }
}