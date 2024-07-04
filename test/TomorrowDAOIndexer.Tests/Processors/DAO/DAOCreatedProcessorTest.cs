using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class DAOCreatedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_AfterFileInfoUploaded_Test()
    {
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
        await CheckFileInfo(DAOIndex);
    }
    
    [Fact]
    public async Task HandleEventAsync_MaxInfo_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);

        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
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
        DAOIndex.FileInfoList.ShouldBeNull();
    }
    
    [Fact]
    public async Task HandleEventAsync_MinInfo_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Id.ShouldBe(DAOId);
        DAOIndex.SubsistStatus.ShouldBe(true);
        
        DAOIndex.Creator.ShouldBe(string.Empty);
        DAOIndex.FileInfoList.ShouldBeNull();
    }

    [Fact]
    public async Task GetContractAddress_Test()
    {
        var contractAddress = DAOCreatedProcessor.GetContractAddress(TomorrowDAOConst.TestNetSideChainId);
        contractAddress.ShouldBe(TomorrowDAOConst.DAOContractAddressTestNetSideChain);
    }
}