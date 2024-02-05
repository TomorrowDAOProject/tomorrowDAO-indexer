using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class SetHighCouncilExecutionSetProcessor : DAOProcessorBase<SetHighCouncilExecutionSet>
{
    public SetHighCouncilExecutionSetProcessor(ILogger<DAOProcessorBase<SetHighCouncilExecutionSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, DAORepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(SetHighCouncilExecutionSet eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var highCouncilExecutionConfig = eventValue.HighCouncilExecutionConfig;
        Logger.LogInformation("[SetHighCouncilExecutionSet] START: Id={Id}, ChainId={ChainId}, HighCouncilExecutionConfig={HighCouncilExecutionConfig}",
            DAOId, chainId, highCouncilExecutionConfig);
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[SetHighCouncilExecutionSet] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.HighCouncilExecutionConfig = highCouncilExecutionConfig;
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[SetHighCouncilExecutionSet] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[SetHighCouncilExecutionSet] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}