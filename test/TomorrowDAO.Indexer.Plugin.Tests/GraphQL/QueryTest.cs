using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public partial class QueryTest : QueryTestBase
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
        
        DAOInfoDto.FileInfoList.ShouldBeNull();
        DAOInfoDto.IsTreasuryContractNeeded.ShouldBe(false);
        DAOInfoDto.SubsistStatus.ShouldBe(true);
        DAOInfoDto.Id.ShouldBe(DAOId);
        DAOInfoDto.Creator.ShouldBe(DAOCreator);
        DAOInfoDto.BlockHeight.ShouldBe(BlockHeight);
        DAOInfoDto.GovernanceMechanism.ShouldBe(GovernanceMechanism.Organization);
    }
    
    [Fact]
    public async Task GetDaoCountAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);

        var count = await Query.GetDaoCountAsync(DAOIndexRepository, ObjectMapper, new GetDaoCountInput
        {
            ChainId = ChainAelf,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        count.ShouldBe(1);
    }
    
    [Fact]
    public async Task HandleEventAsync_UpdateDaoAmount()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        await MockEventProcess(TokenTransferred(), TransferredProcessor);
        
        var daoId = HashHelper.ComputeFrom(Id1).ToHex();
        var daoIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(daoId, ChainAelf);
        daoIndex.ShouldNotBeNull();
        daoIndex.VoteAmount.ShouldBe(100);
        daoIndex.WithdrawAmount.ShouldBe(10);

        var list = await Query.GetDAOAmountRecordAsync(DAOIndexRepository, TreasuryFundRepository, ObjectMapper, new GetDAOAmountRecordInput
        {
            ChainId = ChainAelf
        });
        list.Sum(x => x.Amount).ShouldBe(100000090);
    }

    [Fact]
    public async Task HandleEventAsync_GetMyParticipatedAsync()
    {
        await GetVoteRecordCountAsyncTest();
        await GetProposalCountAsyncTest();

        var proposerResult = await Query.GetMyParticipatedAsync(DAOIndexRepository, LatestParticipatedIndexRepository, ObjectMapper,
            new GetParticipatedInput
            {
                ChainId = ChainAelf,
                SkipCount = 0,
                MaxResultCount = 10,
                Address = DAOCreator
            });
        proposerResult.TotalCount.ShouldBe(1);
        var proposerParticipatedList = proposerResult.Data;
        proposerParticipatedList.Count.ShouldBe(1);
        var proposerDao = proposerParticipatedList[0];
        proposerDao.Id.ShouldBe(DAOId);
        
        var voteResult = await Query.GetMyParticipatedAsync(DAOIndexRepository, LatestParticipatedIndexRepository, ObjectMapper,
            new GetParticipatedInput
            {
                ChainId = ChainAelf,
                SkipCount = 0,
                MaxResultCount = 10,
                Address = User
            });
        voteResult.TotalCount.ShouldBe(1);
        var voteParticipatedList = voteResult.Data;
        voteParticipatedList.Count.ShouldBe(1);
        var voteDao = voteParticipatedList[0];
        voteDao.Id.ShouldBe(DAOId);
    }
}