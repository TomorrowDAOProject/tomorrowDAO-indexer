using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateInfoUpdatedProcessor : ElectionProcessorBase<CandidateInfoUpdated>, ISingletonDependency
{
    public override async Task ProcessAsync(CandidateInfoUpdated logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = logEvent.CandidateAddress?.ToBase58();
        Logger.LogInformation("[CandidateInfoUpdated] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            daoId, chainId, candidate);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, daoId, candidate, CandidateTerm);
            var electionIndex = await GetEntityAsync<ElectionIndex>(id);
            if (electionIndex == null)
            {
                Logger.LogInformation(
                    "[CandidateInfoUpdated] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
                    daoId, chainId, candidate);
                return;
            }

            if (logEvent.IsEvilNode)
            {
                electionIndex.HighCouncilType = HighCouncilType.BlackList;
                await SaveEntityAsync(electionIndex, context);
                Logger.LogInformation(
                    "[CandidateInfoUpdated] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId,
                    candidate);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[CandidateInfoUpdated] Exception Id={DAOId}, ChainId={ChainId}, Candidate={candidate}",
                daoId, chainId, candidate);
            throw;
        }
    }
}