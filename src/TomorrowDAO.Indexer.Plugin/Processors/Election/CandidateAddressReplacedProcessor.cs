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

public class CandidateAddressReplacedProcessor : ElectionProcessorBase<CandidateAddressReplaced>
{
    public CandidateAddressReplacedProcessor(
        ILogger<AElfLogEventProcessorBase<CandidateAddressReplaced, LogEventInfo>> logger, IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository,
        IElectionProvider electionProvider) : base(logger, objectMapper,
        contractInfoOptions, electionRepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(CandidateAddressReplaced eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[CandidateAddressReplaced] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var oldCandidate = eventValue.OldAddress?.ToBase58();
            var newCandidate = eventValue.NewAddress?.ToBase58();
            var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, DAOId, oldCandidate, CandidateTerm), chainId);
            if (electionIndex == null)
            {
                Logger.LogInformation(
                    "[CandidateAddressReplaced] oldCandidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}",
                    DAOId, chainId, oldCandidate);
                return;
            }

            await ElectionRepository.DeleteAsync(electionIndex);
            await SaveIndexAsync(new ElectionIndex
            {
                Address = newCandidate,
                DAOId = DAOId,
                TermNumber = CandidateTerm,
                HighCouncilType = HighCouncilType.Candidate,
                Id = IdGenerateHelper.GetId(chainId, DAOId, newCandidate, CandidateTerm)
            }, context);
            Logger.LogInformation(
                "[CandidateAddressReplaced] FINISH: Id={Id}, ChainId={ChainId}, oldCandidate={candidate} newCandidate{}",
                DAOId, chainId, oldCandidate, newCandidate);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[CandidateAddressReplaced] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}