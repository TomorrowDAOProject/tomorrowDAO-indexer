using AElf;
using AElf.Types;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;
using Xunit;
using GovernanceMechanism = TomorrowDAO.Indexer.Plugin.Enums.GovernanceMechanism;

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
    
    [Fact]
    public async Task EmptySocialMedia_Test()
    {
        //empty
        var metadata = new TomorrowDAO.Contracts.DAO.Metadata
        {
            Name = DAOName,
            LogoUrl = DAOLogoUrl,
            Description = DAODescription,
            SocialMedia = {}
        };

        var daoCreated = new DAOCreated
        {
            Metadata = metadata,
            GovernanceToken = Elf,
            DaoId = HashHelper.ComputeFrom(Id1),
            Creator = Address.FromBase58(DAOCreator),
            ContractAddressList = new ContractAddressList
            {
                TreasuryContractAddress = Address.FromBase58(TreasuryContractAddress),
                VoteContractAddress = Address.FromBase58(VoteContractAddress),
                ElectionContractAddress = Address.FromBase58(ElectionContractAddress),
                GovernanceContractAddress = Address.FromBase58(GovernanceContractAddress),
                TimelockContractAddress = Address.FromBase58(TimelockContractAddress)
            },
            IsNetworkDao = false,
            GovernanceMechanism = TomorrowDAO.Contracts.DAO.GovernanceMechanism.Referendum
        };
        
        await MockEventProcess(daoCreated, DAOCreatedProcessor);
        
        var DAOIndex = await GetIndexById<DAOIndex>(daoCreated.DaoId.ToHex());
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Id.ShouldBe(DAOId);
        DAOIndex.SocialMedia.ShouldNotBeNull();
        DAOIndex.SocialMedia.ShouldBe("{ }");
        
        
        //null
        metadata = new TomorrowDAO.Contracts.DAO.Metadata
        {
            Name = DAOName,
            LogoUrl = DAOLogoUrl,
            Description = DAODescription
        };
        daoCreated.Metadata = metadata;
        daoCreated.DaoId = HashHelper.ComputeFrom(Id2);
        await MockEventProcess(daoCreated, DAOCreatedProcessor);
        
        DAOIndex = await GetIndexById<DAOIndex>(daoCreated.DaoId.ToHex());
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Id.ShouldBe(daoCreated.DaoId.ToHex());
        DAOIndex.SocialMedia.ShouldNotBeNull();
        DAOIndex.SocialMedia.ShouldBe("{ }");
    }
}