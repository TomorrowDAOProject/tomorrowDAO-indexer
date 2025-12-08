using Shouldly;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Treasury;

public class TreasuryCreatedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        
        var daoIndex = await GetIndexById<DAOIndex>(DAOId);
        daoIndex.ShouldNotBeNull();
        daoIndex.TreasuryAccountAddress.ShouldBe(TreasuryAccountAddress);
    }
    
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
    }
}