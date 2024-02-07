using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

public class TreasuryContractSetProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(new TreasuryContractSet
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        }.ToLogEvent(), TreasuryContractSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(new TreasuryContractSet
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            TreasuryContract = Address.FromBase58(DAOCreator)
        }.ToLogEvent(), TreasuryContractSetProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.TreasuryContractAddress.ShouldBe(DAOCreator);
    }
}