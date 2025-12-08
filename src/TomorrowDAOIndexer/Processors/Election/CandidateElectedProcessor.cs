using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateElectedProcessor : ElectionProcessorBase<CandidateElected>
{
    public override async Task ProcessAsync(CandidateElected logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var termNumber = logEvent.PreTermNumber;
        Logger.LogInformation("[CandidateElected] START: DaoId={Id}, ChainId={ChainId}, TermNumber={TermNumber}",
            daoId, chainId, termNumber);
        try
        {
            var candidateElectedId = IdGenerateHelper.GetId(daoId, termNumber, chainId);

            var candidateElectedIndex = await GetEntityAsync<ElectionCandidateElectedIndex>(candidateElectedId);
            if (candidateElectedIndex != null)
            {
                Logger.LogError(
                    "[CandidateElected] CandidateElectedIndex existed: DaoId={Id}, ChainId={ChainId}",
                    daoId, chainId);
                return;
            }

            candidateElectedIndex = ObjectMapper.Map<CandidateElected, ElectionCandidateElectedIndex>(logEvent);
            candidateElectedIndex.Id = candidateElectedId;
            candidateElectedIndex.CandidateElectedTime = context.Block.BlockTime;

            await SaveEntityAsync(candidateElectedIndex, context);
            Logger.LogInformation(
                "[CandidateElected] FINISH: Id={Id}, ChainId={ChainId}", candidateElectedIndex, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[HighCouncilAdded] Exception Id={DAOId}, ChainId={ChainId}",
                daoId, chainId);
            throw;
        }
    }
}