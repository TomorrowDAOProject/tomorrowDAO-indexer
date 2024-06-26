using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

[CollectionDefinition(ClusterCollection.Name)]
public class DAOCreatedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_AfterFileInfoUploaded_Test()
    {
        await MockEventProcess(FileInfosUploaded(), FileInfosUploadedProcessor);
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        
        await CheckFileInfo();
    }

    [Fact]
    public async Task HandleEventAsync_MaxInfo_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        
        var metadata = DAOIndex.Metadata;
        metadata.ShouldNotBeNull();
        metadata.Name.ShouldBe(DAOName);
        metadata.LogoUrl.ShouldBe(DAOLogoUrl);
        metadata.Description.ShouldBe(DAODescription);
        metadata.SocialMedia.ShouldBe(DAOSocialMedia);
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
        DAOIndex.BlockHeight.ShouldBe(BlockHeight);
        DAOIndex.ActiveTimePeriod.ShouldBe(MinActiveTimePeriod);
        DAOIndex.VetoActiveTimePeriod.ShouldBe(MinVetoActiveTimePeriod);
        DAOIndex.PendingTimePeriod.ShouldBe(MinPendingTimePeriod);
        DAOIndex.ExecuteTimePeriod.ShouldBe(MinExecuteTimePeriod);
        DAOIndex.VetoExecuteTimePeriod.ShouldBe(MinVetoExecuteTimePeriod);
        DAOIndex.GovernanceMechanism.ShouldBe(GovernanceMechanism.Organization);
    }

    [Fact]
    public async Task HandleEventAsync_MinInfo_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Id.ShouldBe(DAOId);
        DAOIndex.SubsistStatus.ShouldBe(true);
        
        DAOIndex.Creator.ShouldBe(string.Empty);
        DAOIndex.FileInfoList.ShouldBeNull();
    }

    [Fact]
    public async Task GetContractAddress_Test()
    {
        var processor = GetRequiredService<DAOCreatedProcessor>();
        var contractAddress = processor.GetContractAddress(ChainAelf);
        contractAddress.ShouldBe("DAOContractAddress");
    }
}