using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Election;

public class CandidateElectedProcessor : ElectionProcessorBase<CandidateElected>
{
    public CandidateElectedProcessor(
        ILogger<AElfLogEventProcessorBase<CandidateElected, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger, objectMapper,
        contractInfoOptions, electionRepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(CandidateElected eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var termNumber = eventValue.PreTermNumber;
        Logger.LogInformation("[CandidateElected] START: DaoId={Id}, ChainId={ChainId}, TermNumber={TermNumber}", daoId,
            chainId, termNumber);
        try
        {
            var candidateElectedId = IdGenerateHelper.GetId(daoId, termNumber, chainId);

            var candidateElectedIndex =
                await ElectionProvider.GetCandidateElectedIndexAsync(candidateElectedId, chainId);
            if (candidateElectedIndex != null)
            {
                Logger.LogError(
                    "[CandidateElected] CandidateElectedIndex existed: DaoId={Id}, ChainId={ChainId}",
                    daoId, chainId);
                return;
            }

            candidateElectedIndex = ObjectMapper.Map<CandidateElected, ElectionCandidateElectedIndex>(eventValue);
            candidateElectedIndex.Id = candidateElectedId;
            candidateElectedIndex.CandidateElectedTime = context.BlockTime;

            await ElectionProvider.SaveCandidateElectedIndexAsync(candidateElectedIndex, context);
            Logger.LogInformation(
                "[CandidateElected] FINISH: Id={Id}, ChainId={ChainId}", candidateElectedIndex, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e,
                "[HighCouncilAdded] Exception Id={DAOId}, ChainId={ChainId}", daoId,
                chainId);
            throw;
        }
    }
}