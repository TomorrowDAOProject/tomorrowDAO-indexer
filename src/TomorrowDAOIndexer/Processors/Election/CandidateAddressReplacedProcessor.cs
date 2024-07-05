using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateAddressReplacedProcessor : ElectionProcessorBase<CandidateAddressReplaced>
{
    public override async Task ProcessAsync(CandidateAddressReplaced logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[CandidateAddressReplaced] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var oldCandidate = logEvent.OldAddress?.ToBase58();
            var newCandidate = logEvent.NewAddress?.ToBase58();
            var id = IdGenerateHelper.GetId(chainId, daoId, oldCandidate, CandidateTerm);
            var electionIndex = await GetEntityAsync<ElectionIndex>(id);
            if (electionIndex == null)
            {
                Logger.LogInformation(
                    "[CandidateAddressReplaced] oldCandidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
                    daoId, chainId, oldCandidate);
                return;
            }

            await DeleteEntityAsyncAndCheck<ElectionIndex>(id);

            var newId = IdGenerateHelper.GetId(chainId, daoId, newCandidate, CandidateTerm);
            electionIndex = new ElectionIndex
            {
                Address = newCandidate,
                DAOId = daoId,
                TermNumber = CandidateTerm,
                HighCouncilType = HighCouncilType.Candidate,
                Id = newId
            };
            await SaveEntityAsync(electionIndex, context);

            Logger.LogInformation(
                "[CandidateAddressReplaced] FINISH: Id={Id}, ChainId={ChainId}, oldCandidate={candidate} newCandidate{}",
                daoId, chainId, oldCandidate, newCandidate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[CandidateAddressReplaced] Exception Id={DAOId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}