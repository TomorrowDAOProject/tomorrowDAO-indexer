using AElf;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using Xunit;

namespace TomorrowDAOIndexer;

public class FileInfosRemovedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_MinInfo_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(FileInfosRemoved(), FileInfosRemovedProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.FileInfoList.ShouldBe("[]");
    }
    
    [Fact]
    public async Task HandleEventAsync_DAONotExisted_Test()
    {
        await MockEventProcess(FileInfosRemoved(), FileInfosRemovedProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Exception_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(new FileInfosRemoved { DaoId = HashHelper.ComputeFrom(Id1) }, FileInfosRemovedProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        DAOIndex.ShouldNotBeNull();
    }
}