using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

public class VoteContractSetProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(new VoteContractSet()
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        }.ToLogEvent(), VoteContractSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(new VoteContractSet()
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            VoteContract = Address.FromBase58(DAOCreator)
        }.ToLogEvent(), VoteContractSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.VoteContractAddress.ShouldBe(DAOCreator);
    }
}