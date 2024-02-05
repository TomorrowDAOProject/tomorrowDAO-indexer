using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;
using HighCouncilConfig = TomorrowDAO.Indexer.Plugin.Entities.HighCouncilConfig;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class HighCouncilDisabledProcessor : DAOProcessorBase<HighCouncilDisabled>
{
    public HighCouncilDisabledProcessor(ILogger<DAOProcessorBase<HighCouncilDisabled>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, DAORepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilDisabled eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilDisabled] START: Id={Id}, ChainId={ChainId}",
            DAOId, chainId);
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[HighCouncilDisabled] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.IsHighCouncilEnabled = false;
            DAOIndex.HighCouncilConfig = new HighCouncilConfig();
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[HighCouncilDisabled] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilDisabled] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}