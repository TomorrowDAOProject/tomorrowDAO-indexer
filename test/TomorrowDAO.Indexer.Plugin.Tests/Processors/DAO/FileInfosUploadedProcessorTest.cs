using Newtonsoft.Json;
using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using Xunit;
using FileInfoIndexer = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

[CollectionDefinition(ClusterCollection.Name)]
public class FileInfosUploadedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_BeforeDaoCreated_Test()
    {
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        await CheckFileInfo();
    }

    [Fact]
    public async Task HandleEventAsync_FirstAdd_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        
        await CheckFileInfo();
    }
    
    [Fact]
    public async Task HandleEventAsync_DuplicateAdd_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);

        await CheckFileInfo();
    }
}