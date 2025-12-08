using AElf;
using Shouldly;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest : TomorrowDAOIndexerTestBase
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
       
        DAOInfoDto.Metadata.Name.ShouldBe(DAOName);
        DAOInfoDto.Metadata.LogoUrl.ShouldBe(DAOLogoUrl);
        DAOInfoDto.Metadata.Description.ShouldBe(DAODescription);
        DAOInfoDto.Metadata.SocialMedia.ShouldBe(DAOSocialMedia);
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

    [Fact]
    public async Task GetDaoCountAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);

        var result = await Query.GetDaoCountAsync(DAOIndexRepository, new GetDaoCountInput
        {
            ChainId = ChainId,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetDAOAmountRecordAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        await MockEventProcess(TreasuryCreated(), TreasuryCreatedProcessor);
        // await MockEventProcess(TokenTransferred(), TransferredProcessor);
        await TransferredProcessor.ProcessAsync(GenerateLogEventContext(TokenTransferred()));
        
        var daoIndex = await GetIndexById<DAOIndex>(DAOId);
        daoIndex.ShouldNotBeNull();
        daoIndex.VoteAmount.ShouldBe(100);
        daoIndex.WithdrawAmount.ShouldBe(10);
    
        var list = await Query.GetDAOAmountRecordAsync(DAOIndexRepository, TreasuryFundSumRepository, ObjectMapper, new GetDAOAmountRecordInput
        {
            ChainId = ChainId
        });
        list.Sum(x => x.Amount).ShouldBe(100000090);
    }

    [Fact]
    public async Task GetDAOVoterRecordAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(VotingItemRegistered(), VotingItemRegisteredProcessor);
        await MockEventProcess(VoteVoted(), VoteVotedProcessor);

        var result = await Query.GetDAOVoterRecordAsync(DaoVoterRecordIndexRepository, ObjectMapper, new GetDAOVoterRecordInput
        {
            ChainId = ChainId, DaoId = DAOId, VoterAddress = User
        });
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        var daoVoterRecordIndex = result[0];
        daoVoterRecordIndex.Count.ShouldBe(1);
        daoVoterRecordIndex.Amount.ShouldBe(100);
        daoVoterRecordIndex.VoterAddress.ShouldBe(User);
        daoVoterRecordIndex.DaoId.ShouldBe(DAOId);
    }

    [Fact]
    public async Task GetMyParticipatedAsync_Test()
    {
        await GetVoteRecordCountAsync_Test();
        await GetProposalCountAsync_Test();
    
        var proposerResult = await Query.GetMyParticipatedAsync(DAOIndexRepository, LatestParticipatedIndexRepository, ObjectMapper,
            new GetParticipatedInput
            {
                ChainId = ChainId,
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
                ChainId = ChainId,
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