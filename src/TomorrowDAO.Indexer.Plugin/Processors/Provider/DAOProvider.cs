using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Nest;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IDAOProvider
{
    Task<DAOIndex> GetDaoAsync(string chainId, string daoId);

    Task<DAOIndex> GetDaoByTreasuryAddressAsync(string chainId, string treasuryAddress);

    Task<DaoVoterRecordIndex> GetDaoVoterRecordAsync(string chainId, string id);

    Task SaveIndexAsync(DAOIndex index, LogEventContext context);
    Task SaveIndexAsync(OrganizationIndex index, LogEventContext context);
    Task DeleteMemberAsync(string chainId, string id);

    Task SaveDaoVoterRecordAsync(DaoVoterRecordIndex daoVoterRecordIndex, LogEventContext context);
}

public class DAOProvider : IDAOProvider, ISingletonDependency
{
    private readonly ILogger<DAOProvider> _logger;
    private readonly IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> _daoRepository;
    private readonly IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> _organizationRepository;
    private readonly IAElfIndexerClientEntityRepository<DaoVoterRecordIndex, LogEventInfo> _daoVoterRecordRepository;
    private readonly IObjectMapper _objectMapper;

    public DAOProvider(ILogger<DAOProvider> logger,
        IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> organizationRepository,
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<DaoVoterRecordIndex, LogEventInfo> daoVoterRecordRepository)
    {
        _logger = logger;
        _daoRepository = daoRepository;
        _objectMapper = objectMapper;
        _daoVoterRecordRepository = daoVoterRecordRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<DAOIndex> GetDaoAsync(string chainId, string DAOId)
    {
        var DAOIndex = await _daoRepository.GetFromBlockStateSetAsync(DAOId, chainId);
        if (DAOIndex != null)
        {
            return DAOIndex;
        }

        _logger.LogInformation("DAOIndex with id {id} chainId {chainId} not existed.", DAOId, chainId);
        return null;
    }

    public async Task<DAOIndex> GetDaoByTreasuryAddressAsync(string chainId, string treasuryAddress)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<DAOIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(chainId)));
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.TreasuryAccountAddress).Value(treasuryAddress)));

        QueryContainer Filter(QueryContainerDescriptor<DAOIndex> f) =>
            f.Bool(b => b.Must(mustQuery));

        var daoIndex = await _daoRepository.GetAsync(Filter);
        if (daoIndex != null)
        {
            return daoIndex;
        }

        _logger.LogInformation("DAOIndex with treasuryAddress {treasuryAddress} chainId {chainId} not existed.",
            treasuryAddress, chainId);
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

    public async Task SaveIndexAsync(OrganizationIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _organizationRepository.AddOrUpdateAsync(index);
    }

    public async Task DeleteMemberAsync(string chainId, string id)
    {
        var index = await _organizationRepository.GetFromBlockStateSetAsync(id, chainId);
        if (index != null)
        {
            await _organizationRepository.DeleteAsync(index);
        }
    }

    public async Task SaveDaoVoterRecordAsync(DaoVoterRecordIndex daoVoterRecordIndex, LogEventContext context)
    {
        _objectMapper.Map(context, daoVoterRecordIndex);
        await _daoVoterRecordRepository.AddOrUpdateAsync(daoVoterRecordIndex);
    }
}