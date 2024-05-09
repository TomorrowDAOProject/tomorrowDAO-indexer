using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Election;

public class CandidateAddedProcessor : ElectionProcessorBase<CandidateAdded>
{
    public CandidateAddedProcessor(ILogger<AElfLogEventProcessorBase<CandidateAdded, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger, objectMapper,
        contractInfoOptions, electionRepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(CandidateAdded eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = eventValue.Candidate?.ToBase58();
        Logger.LogInformation("[CandidateAdded] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            DAOId, chainId, candidate);
        try
        {
            await SaveIndexAsync(new ElectionIndex
            {
                Address = candidate,
                DAOId = DAOId,
                TermNumber = CandidateTerm,
                HighCouncilType = HighCouncilType.Candidate,
                StakeAmount = eventValue.Amount,
                Id = IdGenerateHelper.GetId(chainId, DAOId, candidate, CandidateTerm)
            }, context);
            Logger.LogInformation("[CandidateAdded] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", DAOId,
                chainId, candidate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[CandidateAdded] Exception Id={DAOId}, ChainId={ChainId}, Candidate={candidate}", DAOId,
                chainId, candidate);
            throw;
        }
    }
}