using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public class DAOQueryTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task GetDAOListAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        
        var DAOs = await Query.GetDAOListAsync(DAOIndexRepository, ObjectMapper, new GetChainBlockHeightInput
        {
            ChainId = ChainId, StartBlockHeight = BlockHeight, EndBlockHeight = BlockHeight + 1,
            SkipCount = 0, MaxResultCount = 10
        });
        DAOs.ShouldNotBeNull();
        DAOs.Count.ShouldBe(1);
        var DAOInfoDto = DAOs[0];
       
        DAOInfoDto.Name.ShouldBe(DAOName);
        DAOInfoDto.LogoUrl.ShouldBe(DAOLogoUrl);
        DAOInfoDto.Description.ShouldBe(DAODescription);
        DAOInfoDto.SocialMedia.ShouldBe(DAOSocialMedia);
        DAOInfoDto.GovernanceToken.ShouldBe(Elf);
        DAOInfoDto.TreasuryContractAddress.ShouldBe(TreasuryContractAddress);
        DAOInfoDto.VoteContractAddress.ShouldBe(VoteContractAddress);
        DAOInfoDto.ElectionContractAddress.ShouldBe(ElectionContractAddress);
        DAOInfoDto.GovernanceContractAddress.ShouldBe(GovernanceContractAddress);
        DAOInfoDto.TimelockContractAddress.ShouldBe(TimelockContractAddress);
        DAOInfoDto.IsTreasuryContractNeeded.ShouldBe(false);
        DAOInfoDto.SubsistStatus.ShouldBe(true);
        DAOInfoDto.Id.ShouldBe(DAOId);
        DAOInfoDto.Creator.ShouldBe(DAOCreator);
        DAOInfoDto.BlockHeight.ShouldBe(BlockHeight);
        DAOInfoDto.GovernanceMechanism.ShouldBe(GovernanceMechanism.Organization);
        DAOInfoDto.ChainId.ShouldBe(ChainId);
        DAOInfoDto.FileInfoList.ShouldBeNull();
    }
}