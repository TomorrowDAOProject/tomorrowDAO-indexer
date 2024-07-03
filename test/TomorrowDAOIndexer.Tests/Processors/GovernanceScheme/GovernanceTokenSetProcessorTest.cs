using Shouldly;
using Xunit;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceTokenSetProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task GovernanceTokenSet_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(GovernanceSchemeAdded(), GovernanceSchemeAddedProcessor);
        await MockEventProcess(GovernanceTokenSet(), GovernanceTokenSetProcessor);
         
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.GovernanceToken.ShouldBe("USDT");
    }
}