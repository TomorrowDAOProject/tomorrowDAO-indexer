using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class CandidateAddedProcessor : ElectionProcessorBase<CandidateAdded>
{
    public CandidateAddedProcessor(ILogger<DaoProcessorBase<CandidateAdded>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(CandidateAdded eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = eventValue.Candidate.ToBase58();
        _logger.LogInformation("[CandidateAdded] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            daoId, chainId, candidate);
        try
        {
            await SaveIndexAsync(new ElectionIndex 
            {
                Address = candidate,
                DaoId = daoId,
                TermNumber = CandidateTerm,
                HighCouncilType = HighCouncilType.Candidate,
                Id = IdGenerateHelper.GetId(chainId, daoId, candidate, CandidateTerm, HighCouncilType.Candidate) 
            }, context);
            _logger.LogInformation("[CandidateAdded] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[CandidateAdded] Exception Id={daoId}, ChainId={ChainId}, Candidate={candidate}", daoId, chainId, candidate);
            throw;
        }
    }
}