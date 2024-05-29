using Shouldly;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class QueryTest : QueryTestBase
{
    [Fact]
    public async Task GetDAOListAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOs = await Query.GetDAOListAsync(DAOIndexRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            ChainId = ChainAelf,
            StartBlockHeight = BlockHeight,
            EndBlockHeight = BlockHeight + 1,
            MaxResultCount = 10
        });
        DAOs.ShouldNotBeNull();
        DAOs.Count.ShouldBe(1);
        var DAOInfoDto = DAOs[0];
        
        var metadata = DAOInfoDto.Metadata;
        metadata.ShouldNotBeNull();
        metadata.Name.ShouldBe(DAOName);
        metadata.LogoUrl.ShouldBe(DAOLogoUrl);
        metadata.Description.ShouldBe(DAODescription);
        metadata.SocialMedia.ShouldBe(DAOSocialMedia);
        DAOInfoDto.GovernanceToken.ShouldBe(Elf);
        DAOInfoDto.TreasuryContractAddress.ShouldBe(TreasuryContractAddress);
        DAOInfoDto.VoteContractAddress.ShouldBe(VoteContractAddress);
        DAOInfoDto.ElectionContractAddress.ShouldBe(ElectionContractAddress);
        DAOInfoDto.GovernanceContractAddress.ShouldBe(GovernanceContractAddress);
        DAOInfoDto.TimelockContractAddress.ShouldBe(TimelockContractAddress);
        
        // DAOInfoDto.HighCouncilConfig.ShouldBeNull();
        DAOInfoDto.FileInfoList.ShouldBeNull();
        DAOInfoDto.IsTreasuryContractNeeded.ShouldBe(false);
        DAOInfoDto.SubsistStatus.ShouldBe(true);
        DAOInfoDto.Id.ShouldBe(DAOId);
        DAOInfoDto.Creator.ShouldBe(DAOCreator);
        DAOInfoDto.BlockHeight.ShouldBe(BlockHeight);
    }
    
    [Fact]
    public async Task GetDaoCountAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);

        var count = await Query.GetDaoCountAsync(DAOIndexRepository, ObjectMapper, new GetDaoCountInput
        {
            ChainId = ChainAelf,
            StartTime = "2024-05-28 00:00:00",
            EndTime = "2024-05-28 23:59:59"
        });
        count.ShouldBe(1);
        
    }
}