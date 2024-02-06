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
    public CandidateAddressReplacedProcessor(ILogger<DAOProcessorBase<CandidateAddressReplaced>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, electionRepository)
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
            var oldCandidate = eventValue.OldAddress.ToBase58();
            var newCandidate = eventValue.NewAddress.ToBase58();
            var electionIndex = await ElectionRepository.GetFromBlockStateSetAsync(IdGenerateHelper
                .GetId(chainId, DAOId, oldCandidate, CandidateTerm, HighCouncilType.Candidate), chainId);
            if (electionIndex == null)
            {
                Logger.LogInformation("[CandidateAddressReplaced] candidate not existed: Id={Id}, ChainId={ChainId}, Candidate={candidate}", DAOId, chainId, oldCandidate);
                return;
            }
            electionIndex.Address = newCandidate;
            await SaveIndexAsync(electionIndex, context);
            Logger.LogInformation("[CandidateAddressReplaced] FINISH: Id={Id}, ChainId={ChainId}, Candidate={candidate}", DAOId, chainId, oldCandidate); 
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[CandidateAddressReplaced] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}