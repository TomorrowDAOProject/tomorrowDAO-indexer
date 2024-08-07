using AElf;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class HighCouncilDisabledProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(new HighCouncilDisabled
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        }, HighCouncilDisabledProcessor);
        
        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(new HighCouncilDisabled
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        }, HighCouncilDisabledProcessor);
        
        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.IsHighCouncilEnabled.ShouldBe(false);
    }
}