using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class CandidateInfoUpdatedProcessor : ElectionProcessorBase<CandidateInfoUpdated>
{
    public CandidateInfoUpdatedProcessor(ILogger<DaoProcessorBase<CandidateInfoUpdated>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(CandidateInfoUpdated eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = eventValue.CandidateAddress.ToBase58();
        _logger.LogInformation("[CandidateInfoUpdated] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            daoId, chainId, candidate);
        try
        {
            var electionIndex = await _electionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, daoId, candidate, CandidateTerm, HighCouncilType.Candidate), chainId);
            if (electionIndex == null)
            {
                _logger.LogInformation("[CandidateInfoUpdated] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
                return;
            }

            if (eventValue.IsEvilNode)
            {
                electionIndex.HighCouncilType = HighCouncilType.BlackList;
                await _electionRepository.DeleteAsync(electionIndex);
                _logger.LogInformation("[CandidateInfoUpdated] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate); 
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[CandidateInfoUpdated] Exception Id={daoId}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
            throw;
        }
    }
}