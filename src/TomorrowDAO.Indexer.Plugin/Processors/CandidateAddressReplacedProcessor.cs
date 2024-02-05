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

public class CandidateAddressReplacedProcessor : ElectionProcessorBase<CandidateAddressReplaced>
{
    public CandidateAddressReplacedProcessor(ILogger<DaoProcessorBase<CandidateAddressReplaced>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(CandidateAddressReplaced eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        _logger.LogInformation("[CandidateAddressReplaced] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var oldCandidate = eventValue.OldAddress.ToBase58();
            var newCandidate = eventValue.NewAddress.ToBase58();
            var electionIndex = await _electionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, daoId, oldCandidate, CandidateTerm, HighCouncilType.Candidate), chainId);
            if (electionIndex == null)
            {
                _logger.LogInformation("[CandidateAddressReplaced] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, oldCandidate);
                return;
            }
            electionIndex.Address = newCandidate;
            await _electionRepository.DeleteAsync(electionIndex);
            _logger.LogInformation("[CandidateAddressReplaced] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, oldCandidate); 
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[CandidateAddressReplaced] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}