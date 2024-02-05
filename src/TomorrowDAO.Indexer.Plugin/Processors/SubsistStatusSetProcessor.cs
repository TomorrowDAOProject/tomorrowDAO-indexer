using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class SubsistStatusSetProcessor : DaoProcessorBase<SubsistStatusSet>
{
    public SubsistStatusSetProcessor(ILogger<DaoProcessorBase<SubsistStatusSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(SubsistStatusSet eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        var subsistStatus = eventValue.Status;
        _logger.LogInformation("[SubsistStatusSet] START: Id={Id}, ChainId={ChainId}, SubsistStatus={SubsistStatus}",
            daoId, chainId, subsistStatus);
        try
        {
            var daoIndex = await _daoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                _logger.LogInformation("[SubsistStatusSet] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex.SubsistStatus = subsistStatus;
            await SaveIndexAsync(daoIndex, context);
            _logger.LogInformation("[SubsistStatusSet] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[SubsistStatusSet] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}