using AElf;
using AElf.CSharp.Core.Extension;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Processors;
using Xunit;
using FileInfo = TomorrowDAO.Contracts.DAO.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

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
        DAOIndex.MetadataAdmin.ShouldBe(DAOMetadataAdmin);
        DAOIndex.GovernanceToken.ShouldBe(Elf);
        DAOIndex.GovernanceSchemeId.ShouldBe(GovernanceSchemeId);

        var highCouncilConfig = DAOIndex.HighCouncilConfig;
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.MaxHighCouncilCandidateCount.ShouldBe(MaxHighCouncilCandidateCount);
        highCouncilConfig.MaxHighCouncilMemberCount.ShouldBe(MaxHighCouncilMemberCount);
        highCouncilConfig.ElectionPeriod.ShouldBe(ElectionPeriod);
        highCouncilConfig.IsRequireHighCouncilForExecution.ShouldBe(true);

        DAOIndex.FileInfoList.ShouldNotBeNull();
        DAOIndex.PermissionAddress.ShouldBeNull();
        DAOIndex.PermissionInfoList.ShouldBeNull();
        DAOIndex.IsTreasuryContractNeeded.ShouldBe(true);
        DAOIndex.IsVoteContractNeeded.ShouldBe(true);
        DAOIndex.TreasuryContractAddress.ShouldBe("TreasuryContractAddress");
        DAOIndex.VoteContractAddress.ShouldBe("VoteContractAddress");
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
        DAOIndex.TreasuryContractAddress.ShouldBe("TreasuryContractAddress");
        DAOIndex.VoteContractAddress.ShouldBe("VoteContractAddress");
        DAOIndex.SubsistStatus.ShouldBe(true);
        
        DAOIndex.Creator.ShouldBeNull();
        DAOIndex.MetadataAdmin.ShouldBeNull();
        DAOIndex.FileInfoList.ShouldBe(string.Empty);
        DAOIndex.HighCouncilConfig.ShouldBeNull();
        DAOIndex.GovernanceSchemeId.ShouldBeNull();
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
    public async Task HandleEventAsync_Exception_Test()
    {
        await MockEventProcess(new DAOCreated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            FileInfoList = new FileInfoList
            {
                Data = { ["cid"] = new FileInfo {File = null} }
            }
        }.ToLogEvent(), DAOCreatedProcessor);
    }

    [Fact]
    public async Task GetContractAddress_Test()
    {
        var processor = GetRequiredService<DAOCreatedProcessor>();
        var contractAddress = processor.GetContractAddress(ChainAelf);
        contractAddress.ShouldBe("DAOContractAddress");
    }
}