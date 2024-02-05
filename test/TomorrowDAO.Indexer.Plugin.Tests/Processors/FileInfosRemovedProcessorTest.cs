using AElf;
using AElf.CSharp.Core.Extension;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

public class FileInfosRemovedProcessorTest : TomorrowDaoIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_MaxInfo_Test()
    {
        await MockEventProcess(MaxInfoDaoCreated(), DaoCreatedProcessor);
        await MockEventProcess(FileInfosRemoved(), FileInfosRemovedProcessor);
        
        var daoIndex = await DaoIndexRepository.GetFromBlockStateSetAsync(DaoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.FileInfoList.ShouldBe("[]");
    }
    
    [Fact]
    public async Task HandleEventAsync_MinInfo_Test()
    {
        await MockEventProcess(MinInfoDaoCreated(), DaoCreatedProcessor);
        await MockEventProcess(FileInfosRemoved(), FileInfosRemovedProcessor);
        
        var daoIndex = await DaoIndexRepository.GetFromBlockStateSetAsync(DaoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.FileInfoList.ShouldBe("");
    }
    
    [Fact]
    public async Task HandleEventAsync_NotExisted_Test()
    {
        await MockEventProcess(FileInfosRemoved(), FileInfosRemovedProcessor);
        
        var daoIndex = await DaoIndexRepository.GetFromBlockStateSetAsync(DaoId, ChainAelf);
        daoIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Exception_Test()
    {
        await MockEventProcess(MaxInfoDaoCreated(), DaoCreatedProcessor);
        await MockEventProcess(new FileInfosRemoved { DaoId = HashHelper.ComputeFrom(Id1) }.ToLogEvent(), FileInfosRemovedProcessor);
        
        var daoIndex = await DaoIndexRepository.GetFromBlockStateSetAsync(DaoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
    }
}