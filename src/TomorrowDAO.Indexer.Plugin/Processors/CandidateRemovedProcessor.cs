using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class CandidateRemovedProcessor : ElectionProcessorBase<CandidateRemoved>
{
    public CandidateRemovedProcessor(ILogger<DaoProcessorBase<CandidateRemoved>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(CandidateRemoved eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = eventValue.Candidate;
        Logger.LogInformation("[CandidateRemoved] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            daoId, chainId, candidate);
        try
        {
            var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, daoId, candidate, CandidateTerm, HighCouncilType.Candidate), chainId);
            if (electionIndex == null)
            {
                Logger.LogInformation("[CandidateRemoved] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
                return;
            }
            await ElectionRepository.DeleteAsync(electionIndex);
            Logger.LogInformation("[CandidateRemoved] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[CandidateRemoved] Exception Id={daoId}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
            throw;
        }
    }
}