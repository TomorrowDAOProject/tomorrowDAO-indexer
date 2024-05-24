using Newtonsoft.Json;
using Shouldly;
using Xunit;
using FileInfoIndexer = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

public class FileInfosUploadedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExisted_Test()
    {
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_FirstAdd_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        var fileInfoListString = DAOIndex.FileInfoList;
        fileInfoListString.ShouldNotBeNull();
        var fileList = JsonConvert.DeserializeObject<List<FileInfoIndexer>>(fileInfoListString);
        fileList.ShouldNotBeNull();
        fileList.Count.ShouldBe(1);
        fileList[0].Uploader.ShouldBe(DAOCreator);
    }
    
    [Fact]
    public async Task HandleEventAsync_DuplicateAdd_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        var fileInfoListString = DAOIndex.FileInfoList;
        fileInfoListString.ShouldNotBeNull();
        var fileList = JsonConvert.DeserializeObject<List<FileInfoIndexer>>(fileInfoListString);
        fileList.ShouldNotBeNull();
        fileList.Count.ShouldBe(1);
        fileList[0].Uploader.ShouldBe(DAOCreator);
    }
}