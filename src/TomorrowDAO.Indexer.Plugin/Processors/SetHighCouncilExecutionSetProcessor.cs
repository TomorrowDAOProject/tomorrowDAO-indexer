using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class SetHighCouncilExecutionSetProcessor : DaoProcessorBase<SetHighCouncilExecutionSet>
{
    public SetHighCouncilExecutionSetProcessor(ILogger<DaoProcessorBase<SetHighCouncilExecutionSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(SetHighCouncilExecutionSet eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var highCouncilExecutionConfig = eventValue.HighCouncilExecutionConfig;
        Logger.LogInformation("[SetHighCouncilExecutionSet] START: Id={Id}, ChainId={ChainId}, HighCouncilExecutionConfig={HighCouncilExecutionConfig}",
            daoId, chainId, highCouncilExecutionConfig);
        try
        {
            var daoIndex = await DaoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                Logger.LogInformation("[SetHighCouncilExecutionSet] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex.HighCouncilExecutionConfig = highCouncilExecutionConfig;
            await SaveIndexAsync(daoIndex, context);
            Logger.LogInformation("[SetHighCouncilExecutionSet] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[SetHighCouncilExecutionSet] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}