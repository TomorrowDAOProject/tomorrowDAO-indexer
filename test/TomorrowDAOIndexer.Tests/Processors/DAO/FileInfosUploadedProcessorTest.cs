using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class FileInfosUploadedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_BeforeDaoCreated_Test()
    {
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        await CheckFileInfo(DAOIndex);
    }

    [Fact]
    public async Task HandleEventAsync_FirstAdd_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        await CheckFileInfo(DAOIndex);
    }
    
    [Fact]
    public async Task HandleEventAsync_DuplicateAdd_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);

        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        await CheckFileInfo(DAOIndex);
    }
}