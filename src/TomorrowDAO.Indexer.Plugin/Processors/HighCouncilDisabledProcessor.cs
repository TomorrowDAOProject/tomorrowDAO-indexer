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

public class HighCouncilDisabledProcessor : DaoProcessorBase<HighCouncilDisabled>
{
    public HighCouncilDisabledProcessor(ILogger<DaoProcessorBase<HighCouncilDisabled>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilDisabled eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        _logger.LogInformation("[HighCouncilDisabled] START: Id={Id}, ChainId={ChainId}",
            daoId, chainId);
        try
        {
            var daoIndex = await _daoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                _logger.LogInformation("[HighCouncilDisabled] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex.IsHighCouncilEnabled = false;
            daoIndex.HighCouncilConfig = new HighCouncilConfig();
            await SaveIndexAsync(daoIndex, context);
            _logger.LogInformation("[HighCouncilDisabled] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[HighCouncilDisabled] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}