using AElf;
using AElf.CSharp.Core.Extension;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Processors;
using Xunit;
using FileInfo = TomorrowDAO.Contracts.DAO.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors;

public class DaoCreatedProcessorTest : TomorrowDaoIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_MaxInfo_Test()
    {
        await MockEventProcess(MaxInfoDaoCreated(), DaoCreatedProcessor);
        
        var daoIndex = await DaoIndexRepository.GetFromBlockStateSetAsync(DaoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        
        var daoMetadata = daoIndex.DaoMetadata;
        daoMetadata.ShouldNotBeNull();
        daoMetadata.Name.ShouldBe(DaoName);
        daoMetadata.LogoUrl.ShouldBe(DaoLogoUrl);
        daoMetadata.Description.ShouldBe(DaoDescription);
        daoMetadata.SocialMedia.ShouldBe(DaoSocialMedia);
        daoIndex.MetadataAdmin.ShouldBe(DaoMetadataAdmin);

        // var governanceSchemeThreshold = daoIndex.GovernanceSchemeThreshold;
        // governanceSchemeThreshold.ShouldNotBeNull();
        // governanceSchemeThreshold.MinimalRequiredThreshold.ShouldBe(MinimalRequiredThreshold);
        // governanceSchemeThreshold.MinimalApproveThreshold.ShouldBe(MinimalApproveThreshold);
        // governanceSchemeThreshold.MinimalVoteThreshold.ShouldBe(MinimalVoteThreshold);
        // governanceSchemeThreshold.MaximalAbstentionThreshold.ShouldBe(MaximalAbstentionThreshold);
        // governanceSchemeThreshold.MaximalRejectionThreshold.ShouldBe(MaximalRejectionThreshold);
        daoIndex.GovernanceToken.ShouldBe(Elf);
        daoIndex.GovernanceSchemeId.ShouldBe(GovernanceSchemeId);

        var highCouncilConfig = daoIndex.HighCouncilConfig;
        highCouncilConfig.ShouldNotBeNull();
        highCouncilConfig.MaxHighCouncilCandidateCount.ShouldBe(MaxHighCouncilCandidateCount);
        highCouncilConfig.MaxHighCouncilMemberCount.ShouldBe(MaxHighCouncilMemberCount);
        highCouncilConfig.ElectionPeriod.ShouldBe(ElectionPeriod);
        highCouncilConfig.IsRequireHighCouncilForExecution.ShouldBe(true);

        daoIndex.FileInfoList.ShouldNotBeNull();
        daoIndex.PermissionAddress.ShouldBeNull();
        daoIndex.PermissionInfoList.ShouldBeNull();
        daoIndex.IsTreasuryContractNeeded.ShouldBe(true);
        daoIndex.IsVoteContractNeeded.ShouldBe(true);
        daoIndex.TreasuryContractAddress.ShouldBe("TreasuryContractAddress");
        daoIndex.VoteContractAddress.ShouldBe("VoteContractAddress");
        daoIndex.SubsistStatus.ShouldBe(true);
        daoIndex.Id.ShouldBe(DaoId);
        daoIndex.Creator.ShouldBe(DaoCreator);
        daoIndex.BlockHeight.ShouldBe(BlockHeight);
    }

    [Fact]
    public async Task HandleEventAsync_MinInfo_Test()
    {
        await MockEventProcess(MinInfoDaoCreated(), DaoCreatedProcessor);
        
        var daoIndex = await DaoIndexRepository.GetFromBlockStateSetAsync(DaoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.Id.ShouldBe(DaoId);
        daoIndex.TreasuryContractAddress.ShouldBe("TreasuryContractAddress");
        daoIndex.VoteContractAddress.ShouldBe("VoteContractAddress");
        daoIndex.SubsistStatus.ShouldBe(true);
        
        daoIndex.Creator.ShouldBeNull();
        daoIndex.MetadataAdmin.ShouldBeNull();
        daoIndex.FileInfoList.ShouldBe(string.Empty);
        daoIndex.HighCouncilConfig.ShouldBeNull();
        daoIndex.GovernanceSchemeId.ShouldBeNull();
        // daoIndex.GovernanceSchemeThreshold.ShouldBeNull();
    }

    [Fact]
    public async Task HandleEventAsync_Duplicate_Test()
    {
        await MockEventProcess(MaxInfoDaoCreated(), DaoCreatedProcessor);
        await MockEventProcess(MinInfoDaoCreated(), DaoCreatedProcessor);
        
        var daoIndex = await DaoIndexRepository.GetFromBlockStateSetAsync(DaoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.Creator.ShouldBe(DaoCreator);
    }
    
    [Fact]
    public async Task HandleEventAsync_Exception_Test()
    {
        await MockEventProcess(new DAOCreated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            FileInfoList = new FileInfoList
            {
                FileInfos = { new FileInfo { File = null } }
            }
        }.ToLogEvent(), DaoCreatedProcessor);
    }

    [Fact]
    public async Task GetContractAddress_Test()
    {
        var processor = GetRequiredService<DaoCreatedProcessor>();
        var contractAddress = processor.GetContractAddress(ChainAelf);
        contractAddress.ShouldBe("DaoContractAddress");
    }
}