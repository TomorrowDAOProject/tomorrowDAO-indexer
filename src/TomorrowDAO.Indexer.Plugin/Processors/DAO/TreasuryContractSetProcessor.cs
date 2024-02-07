using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class TreasuryContractSetProcessor : DAOProcessorBase<TreasuryContractSet>
{
    public TreasuryContractSetProcessor(ILogger<DAOProcessorBase<TreasuryContractSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAORepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(TreasuryContractSet eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var treasuryContract = eventValue.TreasuryContract?.ToBase58();
        Logger.LogInformation("[TreasuryContractSet] START: Id={Id}, ChainId={ChainId}, TreasuryContract={treasuryContract}",
            DAOId, chainId, treasuryContract);
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[TreasuryContractSet] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.TreasuryContractAddress = treasuryContract;
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[TreasuryContractSet] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryContractSet] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}