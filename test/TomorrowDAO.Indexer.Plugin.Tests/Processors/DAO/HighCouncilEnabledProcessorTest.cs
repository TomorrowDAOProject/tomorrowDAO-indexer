using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
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
            DaoId = HashHelper.ComputeFrom(Id1)
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
        }.ToLogEvent(), HighCouncilEnabledProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.IsHighCouncilEnabled.ShouldBe(true);
        DAOIndex.HighCouncilAddress.ShouldBe(Creator);
        // var highCouncilConfig = DAOIndex.HighCouncilConfig;
        // highCouncilConfig.ShouldNotBeNull();
        // highCouncilConfig.MaxHighCouncilMemberCount.ShouldBe(1);
        // highCouncilConfig.ElectionPeriod.ShouldBe(2);
        // highCouncilConfig.MaxHighCouncilCandidateCount.ShouldBe(3);
        // highCouncilConfig.StakingAmount.ShouldBe(4);
    }
}