using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;
using HighCouncilConfigIndexer = TomorrowDAO.Indexer.Plugin.Entities.HighCouncilConfig;
using HighCouncilConfigContract = TomorrowDAO.Contracts.DAO.HighCouncilConfig;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class HighCouncilEnabledProcessor : DaoProcessorBase<HighCouncilEnabled>
{
    public HighCouncilEnabledProcessor(ILogger<DaoProcessorBase<HighCouncilEnabled>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilEnabled eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        _logger.LogInformation("[HighCouncilEnabled] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var daoIndex = await _daoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                _logger.LogInformation("[HighCouncilEnabled] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex.IsHighCouncilEnabled = true;
            var highCouncilConfig = eventValue.HighCouncilConfig;
            if (highCouncilConfig != null)
            {
                daoIndex.HighCouncilConfig = _objectMapper.Map<HighCouncilConfigContract, HighCouncilConfigIndexer>(highCouncilConfig);
            }
            await SaveIndexAsync(daoIndex, context);
            _logger.LogInformation("[HighCouncilEnabled] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[HighCouncilEnabled] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}