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
        _logger.LogInformation("[CandidateRemoved] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            daoId, chainId, candidate);
        try
        {
            var electionIndex = await _electionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, daoId, candidate, CandidateTerm, HighCouncilType.Candidate), chainId);
            if (electionIndex == null)
            {
                _logger.LogInformation("[CandidateRemoved] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
                return;
            }
            await _electionRepository.DeleteAsync(electionIndex);
            _logger.LogInformation("[CandidateRemoved] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[CandidateRemoved] Exception Id={daoId}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
            throw;
        }
    }
}