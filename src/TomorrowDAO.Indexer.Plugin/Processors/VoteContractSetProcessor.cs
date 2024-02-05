using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class VoteContractSetProcessor : DaoProcessorBase<VoteContractSet>
{
    public VoteContractSetProcessor(ILogger<DaoProcessorBase<VoteContractSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(VoteContractSet eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var voteContract = eventValue.VoteContract?.ToBase58();
        Logger.LogInformation("[VoteContractSet] START: Id={Id}, ChainId={ChainId}, VoteContract={voteContract}",
            daoId, chainId, voteContract);
        try
        {
            var daoIndex = await DaoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                Logger.LogInformation("[VoteContractSet] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex.TreasuryContractAddress = voteContract;
            await SaveIndexAsync(daoIndex, context);
            Logger.LogInformation("[VoteContractSet] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[VoteContractSet] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}