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

public class CandidateRemovedProcessor : ElectionProcessorBase<CandidateRemoved>
{
    public CandidateRemovedProcessor(ILogger<AElfLogEventProcessorBase<CandidateRemoved, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger, objectMapper,
        contractInfoOptions, electionRepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(CandidateRemoved eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = eventValue.Candidate?.ToBase58();
        Logger.LogInformation("[CandidateRemoved] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            DAOId, chainId, candidate);
        try
        {
            var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, DAOId, candidate, CandidateTerm), chainId);
            if (electionIndex == null)
            {
                Logger.LogInformation(
                    "[CandidateRemoved] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
                    DAOId, chainId, candidate);
                return;
            }

            await ElectionRepository.DeleteAsync(electionIndex);
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