using AElf;
using AElf.CSharp.Core.Extension;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Orleans.TestBase;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

[CollectionDefinition(ClusterCollection.Name)]
public class SubsistStatusSetProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(new SubsistStatusSet
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        }.ToLogEvent(), SubsistStatusSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
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
        }.ToLogEvent(), SubsistStatusSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.SubsistStatus.ShouldBe(false);
    }
}