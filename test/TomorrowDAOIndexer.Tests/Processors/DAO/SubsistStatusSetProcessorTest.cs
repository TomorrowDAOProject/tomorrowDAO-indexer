using AElf;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class SubsistStatusSetProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(new SubsistStatusSet
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        }, SubsistStatusSetProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(new SubsistStatusSet
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            Status = false
        }, SubsistStatusSetProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.SubsistStatus.ShouldBe(false);
    }
}