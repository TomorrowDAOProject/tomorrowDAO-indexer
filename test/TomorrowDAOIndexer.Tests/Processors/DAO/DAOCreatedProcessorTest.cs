using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class DAOCreatedProcessorTest: TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_MaxInfo_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);

        var queryable = await DAOIndexRepository.GetQueryableAsync();
        var DAOIndex = queryable.Single(a => a.Metadata.ChainId == ChainId);
        DAOIndex.ShouldNotBeNull();
        
        var metadata = DAOIndex.Metadata;
        metadata.ShouldNotBeNull();
        metadata.ChainId.ShouldBe(ChainId);

        var block = metadata.Block;
        block.ShouldNotBeNull();
        block.BlockHeight.ShouldBe(BlockHeight);
        
        DAOIndex.BlockHeight.ShouldBe(BlockHeight);
        DAOIndex.Name.ShouldBe(DAOName);
        DAOIndex.LogoUrl.ShouldBe(DAOLogoUrl);
        DAOIndex.Description.ShouldBe(DAODescription);
        DAOIndex.SocialMedia.ShouldBe(DAOSocialMedia);
        DAOIndex.GovernanceToken.ShouldBe(Elf);
        DAOIndex.TreasuryContractAddress.ShouldBe(TreasuryContractAddress);
        DAOIndex.VoteContractAddress.ShouldBe(VoteContractAddress);
        DAOIndex.ElectionContractAddress.ShouldBe(ElectionContractAddress);
        DAOIndex.GovernanceContractAddress.ShouldBe(GovernanceContractAddress);
        DAOIndex.TimelockContractAddress.ShouldBe(TimelockContractAddress);
        DAOIndex.FileInfoList.ShouldBeNull();
        DAOIndex.IsTreasuryContractNeeded.ShouldBe(false);
        DAOIndex.SubsistStatus.ShouldBe(true);
        DAOIndex.Id.ShouldBe(DAOId);
        DAOIndex.Creator.ShouldBe(DAOCreator);
        DAOIndex.ActiveTimePeriod.ShouldBe(MinActiveTimePeriod);
        DAOIndex.VetoActiveTimePeriod.ShouldBe(MinVetoActiveTimePeriod);
        DAOIndex.PendingTimePeriod.ShouldBe(MinPendingTimePeriod);
        DAOIndex.ExecuteTimePeriod.ShouldBe(MinExecuteTimePeriod);
        DAOIndex.VetoExecuteTimePeriod.ShouldBe(MinVetoExecuteTimePeriod);
        DAOIndex.GovernanceMechanism.ShouldBe(GovernanceMechanism.Organization);
    }
}