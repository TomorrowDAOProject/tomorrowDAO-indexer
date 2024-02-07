using AElfIndexer.Client;
using AElfIndexer.Grains.State.Client;
using TomorrowDAO.Indexer.Plugin.Entities;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

public class ProposalProcessorTests : TomorrowDAOIndexerPluginTestBase
{

    protected readonly IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo>
        _proposalRepository;


    public ProposalProcessorTests()
    {
        _proposalRepository =
            GetRequiredService<IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo>>();
    }
    
}