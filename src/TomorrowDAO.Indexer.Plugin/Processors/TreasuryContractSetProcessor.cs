using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class TreasuryContractSetProcessor : DaoProcessorBase<TreasuryContractSet>
{
    public TreasuryContractSetProcessor(ILogger<DaoProcessorBase<TreasuryContractSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(TreasuryContractSet eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var treasuryContract = eventValue.TreasuryContract?.ToBase58();
        Logger.LogInformation("[TreasuryContractSet] START: Id={Id}, ChainId={ChainId}, TreasuryContract={treasuryContract}",
            daoId, chainId, treasuryContract);
        try
        {
            var daoIndex = await DaoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                Logger.LogInformation("[TreasuryContractSet] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex.TreasuryContractAddress = treasuryContract;
            await SaveIndexAsync(daoIndex, context);
            Logger.LogInformation("[TreasuryContractSet] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryContractSet] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}