using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IOrganizationProvider
{
    Task<OrganizationIndex> GetIndexAsync(string chainId, string organizationAddress);

    Task SaveIndexAsync(OrganizationIndex index, LogEventContext context);
}

public class OrganizationProvider : IOrganizationProvider, ISingletonDependency
{
    protected readonly ILogger<OrganizationProvider> _logger;
    protected readonly IObjectMapper _objectMapper;
    protected readonly IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> _organizationRepository;

    public OrganizationProvider(IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> organizationRepository,
        ILogger<OrganizationProvider> logger)
    {
        _objectMapper = objectMapper;
        _organizationRepository = organizationRepository;
        _logger = logger;
    }

    public async Task<OrganizationIndex> GetIndexAsync(string chainId, string organizationAddress)
    {
        return await _organizationRepository.GetFromBlockStateSetAsync(organizationAddress, chainId);
    }

    public async Task SaveIndexAsync(OrganizationIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _organizationRepository.AddOrUpdateAsync(index);
    }
}