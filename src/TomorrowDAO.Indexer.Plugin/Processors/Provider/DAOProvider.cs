using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IDAOProvider
{
    Task<DAOIndex> GetDAOAsync(string chainId, string DAOId);

    Task<DaoVoterRecordIndex> GetDaoVoterRecordAsync(string chainId, string id);

    Task SaveIndexAsync(DAOIndex index, LogEventContext context);

    Task SaveDaoVoterRecordAsync(DaoVoterRecordIndex daoVoterRecordIndex, LogEventContext context);
}

public class DAOProvider : IDAOProvider, ISingletonDependency
{
    private readonly ILogger<DAOProvider> _logger;
    private readonly IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> _daoRepository;
    private readonly IAElfIndexerClientEntityRepository<DaoVoterRecordIndex, LogEventInfo> _daoVoterRecordRepository;
    private readonly IObjectMapper _objectMapper;

    public DAOProvider(ILogger<DAOProvider> logger,
        IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> daoRepository,
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<DaoVoterRecordIndex, LogEventInfo> daoVoterRecordRepository)
    {
        _logger = logger;
        _daoRepository = daoRepository;
        _objectMapper = objectMapper;
        _daoVoterRecordRepository = daoVoterRecordRepository;
    }

    public async Task<DAOIndex> GetDAOAsync(string chainId, string DAOId)
    {
        var DAOIndex = await _daoRepository.GetFromBlockStateSetAsync(DAOId, chainId);
        if (DAOIndex != null)
        {
            return DAOIndex;
        }

        _logger.LogInformation("DAOIndex with id {id} chainId {chainId} not existed.", DAOId, chainId);
        return null;
    }

    public async Task<DaoVoterRecordIndex> GetDaoVoterRecordAsync(string chainId, string id)
    {
        var daoVoterRecordIndex = await _daoVoterRecordRepository.GetFromBlockStateSetAsync(id, chainId);
        return daoVoterRecordIndex ?? null;
    }

    public async Task SaveIndexAsync(DAOIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _daoRepository.AddOrUpdateAsync(index);
    }

    public async Task SaveDaoVoterRecordAsync(DaoVoterRecordIndex daoVoterRecordIndex, LogEventContext context)
    {
        _objectMapper.Map(context, daoVoterRecordIndex);
        await _daoVoterRecordRepository.AddOrUpdateAsync(daoVoterRecordIndex);
    }
}