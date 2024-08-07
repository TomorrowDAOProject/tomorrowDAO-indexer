using AElf;
using AElf.Types;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;
using Xunit;
using GovernanceSchemeThreshold = TomorrowDAO.Contracts.DAO.GovernanceSchemeThreshold;

namespace TomorrowDAOIndexer.Processors.DAO;

public class HighCouncilEnabledProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_DAONotExist_Test()
    {
        await MockEventProcess(HighCouncilEnabled(), HighCouncilEnabledProcessor);
        await CheckParam();
    }
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(HighCouncilEnabled(), HighCouncilEnabledProcessor);
        await CheckParam();
    }

    private async Task CheckParam()
    {
        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.IsHighCouncilEnabled.ShouldBe(true);
        DAOIndex.HighCouncilAddress.ShouldBe(Creator);
        DAOIndex.MaxHighCouncilMemberCount.ShouldBe(1);
        DAOIndex.ElectionPeriod.ShouldBe(2);
        DAOIndex.MaxHighCouncilCandidateCount.ShouldBe(3);
        DAOIndex.StakingAmount.ShouldBe(4);
    }

    private HighCouncilEnabled HighCouncilEnabled()
    {
        return new HighCouncilEnabled
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            HighCouncilAddress = Address.FromBase58(Creator),
            HighCouncilInput = new HighCouncilInput
            {
                GovernanceSchemeThreshold = new GovernanceSchemeThreshold(),
                HighCouncilConfig = new HighCouncilConfig
                {
                    MaxHighCouncilMemberCount = 1L,
                    ElectionPeriod = 2L,
                    MaxHighCouncilCandidateCount = 3L,
                    StakingAmount = 4L
                }
            }
        };
    }
}