using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using Newtonsoft.Json;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using Xunit;
using PermissionInfoIndexer = TomorrowDAO.Indexer.Plugin.Entities.PermissionInfo;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

public class PermissionsSetProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(new PermissionsSet
        {
            DaoId = HashHelper.ComputeFrom(Id1),
        }.ToLogEvent(), PermissionsSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(new PermissionsSet
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            Here = Address.FromBase58(DAOCreator),
            PermissionInfoList = new PermissionInfoList
            {
                PermissionInfos =
                {
                    new PermissionInfo
                    {
                        PermissionType = PermissionType.Highcouncilonly,
                        What = "What",
                        Who = Address.FromBase58(DAOCreator),
                        Where = Address.FromBase58(DAOCreator)
                    }
                }
            }
        }.ToLogEvent(), PermissionsSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        var permissionInfoList = DAOIndex.PermissionInfoList;
        permissionInfoList.ShouldNotBeNull();
        var list = JsonConvert.DeserializeObject<List<PermissionInfoIndexer>>(permissionInfoList);
        list.ShouldNotBeNull();
        list.Count.ShouldBe(1);
        list[0].Where.ShouldBe(DAOCreator);
    }
}