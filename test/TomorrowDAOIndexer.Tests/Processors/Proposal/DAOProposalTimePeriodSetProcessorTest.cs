using Shouldly;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class DAOProposalTimePeriodSetProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task DAOProposalTimePeriodSet_Test()
    {
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(DaoProposalTimePeriodSet(), DAOProposalTimePeriodSetProcessor);
        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.ActiveTimePeriod.ShouldBe(1);
        DAOIndex.VetoActiveTimePeriod.ShouldBe(5);
        DAOIndex.PendingTimePeriod.ShouldBe(3);
        DAOIndex.ExecuteTimePeriod.ShouldBe(2);
        DAOIndex.VetoExecuteTimePeriod.ShouldBe(4);
    }
}