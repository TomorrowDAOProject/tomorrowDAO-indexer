using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IGovernanceProvider
{
    Task<GovernanceSchemeIndex> GetGovernanceSchemeAsync(string chainId, string id);
}

public class GovernanceProvider : IGovernanceProvider, ISingletonDependency
{
    private readonly ILogger<GovernanceProvider> _logger;
    private readonly IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo>
        _governanceSchemeRepository;
    
    public GovernanceProvider(ILogger<GovernanceProvider> logger,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository)
    {
        _logger = logger;
        _governanceSchemeRepository = governanceSchemeRepository;
    }

    public async Task<GovernanceSchemeIndex> GetGovernanceSchemeAsync(string chainId, string id)
    {
        var governanceSchemeIndex =
            await _governanceSchemeRepository.GetFromBlockStateSetAsync(id, chainId);
        if (governanceSchemeIndex != null)
        {
            return governanceSchemeIndex;
        }

        _logger.LogInformation("GovernanceSchemeIndex with id {id} chainId {chainId} has not existed.", id, chainId);
        return null;
    }
}