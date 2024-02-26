using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Election;

public class VotedProcessor : ElectionProcessorBase<Voted>
{
    public VotedProcessor(ILogger<ElectionProcessorBase<Voted>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : 
        base(logger, objectMapper, contractInfoOptions, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(Voted eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var candidate = eventValue.CandidateAddress.ToBase58();
        Logger.LogInformation("[ElectionVoted] START: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
            DAOId, chainId, candidate);
        try
        {
            var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, DAOId, candidate, CandidateTerm), chainId);
            await SaveIndexAsync(new ElectionIndex 
            {
                Address = candidate,
                DAOId = DAOId,
                TermNumber = CandidateTerm,
                HighCouncilType = HighCouncilType.Candidate,
                VotesAmount = electionIndex == null ? eventValue.Amount : electionIndex.VotesAmount + eventValue.Amount,
                Id = IdGenerateHelper.GetId(chainId, DAOId, candidate, CandidateTerm) 
            }, context);

            Logger.LogInformation("[ElectionVoted] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", DAOId, chainId, candidate); 
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ElectionVoted] Exception Id={Id}, ChainId={ChainId}, Candidate={candidate}", DAOId, chainId, candidate);
            throw;
        }
    }
}