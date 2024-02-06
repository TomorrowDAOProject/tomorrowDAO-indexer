using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface IElectionProvider
{
    Task SaveIndexAsync(ElectionIndex index, LogEventContext context);
}

public class ElectionProvider : IElectionProvider, ISingletonDependency
{
    private readonly IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> _electionRepository;
    private readonly IObjectMapper _objectMapper;

    public ElectionProvider(IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository, 
        IObjectMapper objectMapper)
    {
        _electionRepository = electionRepository;
        _objectMapper = objectMapper;
    }

    public async Task SaveIndexAsync(ElectionIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _electionRepository.AddOrUpdateAsync(index);
    }
}