using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IGovernanceSchemeProvider
{
    Task<GovernanceSubSchemeIndex> GetGovernanceSubSchemeAsync(string chainId, string governanceSchemeId);
}

public class GovernanceSchemeProvider  : IGovernanceSchemeProvider, ISingletonDependency
{
    private readonly ILogger<GovernanceSchemeProvider> _logger;
    private readonly IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> _governanceSubSchemeRepository;

    public GovernanceSchemeProvider(ILogger<GovernanceSchemeProvider> logger, 
        IAElfIndexerClientEntityRepository<GovernanceSubSchemeIndex, LogEventInfo> governanceSubSchemeRepository)
    {
        _logger = logger;
        _governanceSubSchemeRepository = governanceSubSchemeRepository;
    }

    public async Task<GovernanceSubSchemeIndex> GetGovernanceSubSchemeAsync(string chainId, string governanceSchemeId)
    {
        var governanceSubSchemeIndex =
            await _governanceSubSchemeRepository.GetFromBlockStateSetAsync(governanceSchemeId, chainId);
        if (governanceSubSchemeIndex != null)
        {
            return governanceSubSchemeIndex;
        }
        _logger.LogInformation("governanceSubSchemeIndex with id {id} chainId {chainId} has not existed.", governanceSchemeId, chainId);
        return null;
    }
}