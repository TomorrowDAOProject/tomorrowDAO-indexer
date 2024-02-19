using Shouldly;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

public class DAOCreatedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
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

        DAOIndex.HighCouncilConfig.ShouldBeNull();
        DAOIndex.FileInfoList.ShouldBeNull();
        DAOIndex.PermissionAddress.ShouldBeNull();
        DAOIndex.PermissionInfoList.ShouldBeNull();
        DAOIndex.IsTreasuryContractNeeded.ShouldBe(false);
        DAOIndex.IsVoteContractNeeded.ShouldBe(false);
        DAOIndex.SubsistStatus.ShouldBe(true);
        DAOIndex.Id.ShouldBe(DAOId);
        DAOIndex.Creator.ShouldBe(DAOCreator);
        DAOIndex.BlockHeight.ShouldBe(BlockHeight);
    }

    [Fact]
    public async Task HandleEventAsync_MinInfo_Test()
    {
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Id.ShouldBe(DAOId);
        DAOIndex.SubsistStatus.ShouldBe(true);
        
        DAOIndex.Creator.ShouldBeNull();
        DAOIndex.FileInfoList.ShouldBeNull();
        DAOIndex.HighCouncilConfig.ShouldBeNull();
    }

    [Fact]
    public async Task HandleEventAsync_Duplicate_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(MinInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Creator.ShouldBe(DAOCreator);
    }

    [Fact]
    public async Task GetContractAddress_Test()
    {
        var processor = GetRequiredService<DAOCreatedProcessor>();
        var contractAddress = processor.GetContractAddress(ChainAelf);
        contractAddress.ShouldBe("DAOContractAddress");
    }
}