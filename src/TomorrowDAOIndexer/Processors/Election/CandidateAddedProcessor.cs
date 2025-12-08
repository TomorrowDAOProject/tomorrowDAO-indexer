using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Election;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.Election;

public class CandidateAddedProcessor : ElectionProcessorBase<CandidateAdded>
{
    public override async Task ProcessAsync(CandidateAdded logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = logEvent.Candidate?.ToBase58();
        Logger.LogInformation("[CandidateAdded] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            DAOId, chainId, candidate);
        try
        {
            await SaveEntityAsync(new ElectionIndex
            {
                Address = candidate,
                DAOId = DAOId,
                TermNumber = CandidateTerm,
                HighCouncilType = HighCouncilType.Candidate,
                StakeAmount = logEvent.Amount,
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