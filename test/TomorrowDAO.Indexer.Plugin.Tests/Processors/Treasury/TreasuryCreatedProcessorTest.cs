using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Treasury;

[CollectionDefinition(ClusterCollection.Name)]
public class TreasuryCreatedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        
        var daoIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.TreasuryAccountAddress.ShouldBe(TreasuryAccountAddress);
    }
    
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
    }
}