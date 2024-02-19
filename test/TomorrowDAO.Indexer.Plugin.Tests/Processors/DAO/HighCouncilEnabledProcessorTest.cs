using AElf;
using AElf.CSharp.Core.Extension;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

public class HighCouncilEnabledProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(new HighCouncilEnabled
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            ExecutionConfig = true
        }.ToLogEvent(), HighCouncilEnabledProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(new HighCouncilEnabled
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            ExecutionConfig = true
        }.ToLogEvent(), HighCouncilEnabledProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.IsHighCouncilEnabled.ShouldBe(true);
    }
}