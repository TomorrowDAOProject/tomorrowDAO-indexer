using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using HighCouncilConfigIndexer = TomorrowDAO.Indexer.Plugin.Entities.HighCouncilConfig;
using HighCouncilConfigContract = TomorrowDAO.Contracts.DAO.HighCouncilConfig;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class HighCouncilConfigSetProcessor : DAOProcessorBase<HighCouncilConfigSet>
{
    public HighCouncilConfigSetProcessor(ILogger<DAOProcessorBase<HighCouncilConfigSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAORepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilConfigSet eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilConfigSet] START: Id={Id}, ChainId={ChainId}",
            DAOId, chainId);
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[HighCouncilConfigSet] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.HighCouncilConfig = ObjectMapper.Map<HighCouncilConfigContract, HighCouncilConfigIndexer>(eventValue.HighCouncilConfig);
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[HighCouncilConfigSet] FINISH: Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilConfigSet] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}