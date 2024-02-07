using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IGovernanceProvider
{
    Task<GovernanceSubSchemeIndex> GetGovernanceSubSchemeAsync(string chainId, string governanceSchemeId);
    
    Task<int> GetOrganizationMemberCountAsync(string chainId, string organizationAddress);
}

public class GovernanceProvider : IGovernanceProvider, ISingletonDependency
{
    private readonly ILogger<IGovernanceProvider> _logger;

    private readonly IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo>
        _governanceSubSchemeRepository;

    private readonly IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo>
        _organizationRepository;
    
    public GovernanceProvider(ILogger<IGovernanceProvider> logger,
        IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository, 
        IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> organizationRepository)
    {
        _logger = logger;
        _governanceSubSchemeRepository = governanceSubSchemeRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<GovernanceSubSchemeIndex> GetGovernanceSubSchemeAsync(string chainId, string governanceSchemeId)
    {
        var governanceSubSchemeIndex =
            await _governanceSubSchemeRepository.GetFromBlockStateSetAsync(governanceSchemeId, chainId);
        if (governanceSubSchemeIndex != null)
        {
            return governanceSubSchemeIndex;
        }

        _logger.LogInformation("GovernanceSubSchemeIndex with id {id} chainId {chainId} has not existed.",
            governanceSchemeId, chainId);
        return null;
    }

    public async Task<int> GetOrganizationMemberCountAsync(string chainId, string organizationAddress)
    {
        var organizationIndex =
            await _organizationRepository.GetFromBlockStateSetAsync(organizationAddress, chainId);
        if (organizationIndex != null)
        {
            return organizationIndex.OrganizationMemberSet?.Count ?? 0;
        }

        _logger.LogInformation("OrganizationIndex with id {id} chainId {chainId} has not existed.",
            organizationAddress, chainId);
        return 0;
    }
}