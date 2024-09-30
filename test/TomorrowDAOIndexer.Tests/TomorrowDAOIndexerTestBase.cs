using AeFinder.App.TestBase;
using AeFinder.Sdk;
using AeFinder.Sdk.Entities;
using AeFinder.Sdk.Processor;
using AElf;
using AElf.Contracts.MultiToken;
using AElf.Contracts.Referendum;
using AElf.CSharp.Core;
using AElf.Standards.ACS3;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Contracts.Election;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.DAO;
using TomorrowDAOIndexer.Processors.Election;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using TomorrowDAOIndexer.Processors.NetworkDao.Association;
using TomorrowDAOIndexer.Processors.NetworkDao.Parliament;
using TomorrowDAOIndexer.Processors.NetworkDao.Referendum;
using TomorrowDAOIndexer.Processors.Proposal;
using TomorrowDAOIndexer.Processors.Token;
using TomorrowDAOIndexer.Processors.Treasury;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;
using GovernanceMechanism = TomorrowDAO.Contracts.DAO.GovernanceMechanism;
using FileInfoIndexer = TomorrowDAOIndexer.Entities.FileInfo;
using Voted = TomorrowDAO.Contracts.Election.Voted;
using VotingItem = TomorrowDAO.Contracts.Election.VotingItem;
using AddressListDAO = TomorrowDAO.Contracts.DAO.AddressList;
using Transaction = AeFinder.Sdk.Processor.Transaction;
using ProposalCreated = TomorrowDAO.Contracts.Governance.ProposalCreated;
using ProposalStatus = TomorrowDAO.Contracts.Governance.ProposalStatus;
using ProposalType = TomorrowDAO.Contracts.Governance.ProposalType;
using Vote = TomorrowDAOIndexer.Processors.Vote;
using VoteMechanism = TomorrowDAO.Contracts.Vote.VoteMechanism;
using VoteOption = TomorrowDAO.Contracts.Vote.VoteOption;
using VoteStrategy = TomorrowDAO.Contracts.Vote.VoteStrategy;

namespace TomorrowDAOIndexer;

public abstract class TomorrowDAOIndexerTestBase : AeFinderAppTestBase<TomorrowDAOIndexerTestModule>
{
    protected const string TransactionId = "4e07408562bedb8b60ce05c1decfe3ad16b72230967de01f640b7e4729b49fce";

    protected readonly IAbpLazyServiceProvider LazyServiceProvider;

    // processor
    protected readonly Vote.VoteSchemeCreatedProcessor VoteSchemeCreatedProcessor;
    protected readonly Vote.VotingItemRegisteredProcessor VotingItemRegisteredProcessor;
    protected readonly Vote.VoteWithdrawnProcessor VoteWithdrawnProcessor;
    protected readonly DAOCreatedProcessor DAOCreatedProcessor;
    protected readonly MetadataUpdatedProcessor MetadataUpdatedProcessor;
    protected readonly FileInfosRemovedProcessor FileInfosRemovedProcessor;
    protected readonly FileInfosUploadedProcessor FileInfosUploadedProcessor;
    protected readonly MemberAddedProcessor MemberAddedProcessor;
    protected readonly MemberRemovedProcessor MemberRemovedProcessor;
    protected readonly HighCouncilDisabledProcessor HighCouncilDisabledProcessor;
    protected readonly HighCouncilEnabledProcessor HighCouncilEnabledProcessor;
    protected readonly SubsistStatusSetProcessor SubsistStatusSetProcessor;
    protected readonly TransferredProcessor TransferredProcessor;
    protected readonly IssuedProcessor IssuedProcessor;
    protected readonly BurnedProcessor BurnedProcessor;
    protected readonly TreasuryCreatedProcessor TreasuryCreatedProcessor;
    protected readonly TreasuryTransferredProcessor TreasuryTransferredProcessor;
    protected readonly CandidateAddedProcessor CandidateAddedProcessor;
    protected readonly CandidateAddressReplacedProcessor CandidateAddressReplacedProcessor;
    protected readonly CandidateInfoUpdatedProcessor CandidateInfoUpdatedProcessor;
    protected readonly CandidateRemovedProcessor CandidateRemovedProcessor;
    protected readonly VotedProcessor VotedProcessor;
    protected readonly Vote.VotedProcessor VoteVotedProcessor;
    protected readonly ElectionVotingEventRegisteredProcessor ElectionVotingEventRegisteredProcessor;
    protected readonly HighCouncilAddedProcessor HighCouncilAddedProcessor;
    protected readonly HighCouncilRemovedProcessor HighCouncilRemovedProcessor;
    protected readonly CandidateElectedProcessor CandidateElectedProcessor;
    protected readonly GovernanceSchemeAddedProcessor GovernanceSchemeAddedProcessor;
    protected readonly GovernanceSchemeThresholdRemovedProcessor GovernanceSchemeThresholdRemovedProcessor;
    protected readonly GovernanceSchemeThresholdUpdatedProcessor GovernanceSchemeThresholdUpdatedProcessor;
    protected readonly GovernanceTokenSetProcessor GovernanceTokenSetProcessor;
    protected readonly ProposalCreatedProcessor ProposalCreatedProcessor;
    protected readonly DAOProposalTimePeriodSetProcessor DAOProposalTimePeriodSetProcessor;
    protected readonly ProposalExecutedProcessor ProposalExecutedProcessor;
    protected readonly ProposalVetoedProcessor ProposalVetoedProcessor;
    protected readonly ParliamentProposalCreatedProcessor ParliamentProposalCreatedProcessor;
    protected readonly AssociationProposalCreatedProcessor AssociationProposalCreatedProcessor;
    protected readonly ReferendumProposalCreatedProcessor ReferendumProposalCreatedProcessor;
    protected readonly AssociationOrgCreatedProcessor AssociationOrgCreatedProcessor;
    protected readonly AssociationOrgMemberAddedProcessor AssociationOrgMemberAddedProcessor;
    protected readonly AssociationOrgMemberChangedProcessor AssociationOrgMemberChangedProcessor;
    protected readonly AssociationOrgMemberRemovedProcessor AssociationOrgMemberRemovedProcessor;
    protected readonly AssociationOrgThresholdChangedProcessor AssociationOrgThresholdChangedProcessor;
    protected readonly AssociationOrgWhiteListChangedProcessor AssociationOrgWhiteListChangedProcessor;
    protected readonly AssociationProposalReleasedProcessor AssociationProposalReleasedProcessor;
    protected readonly AssociationReceiptCreatedProcessor AssociationReceiptCreatedProcessor;
    protected readonly ParliamentOrgCreatedProcessor ParliamentOrgCreatedProcessor;
    protected readonly ParliamentOrgThresholdChangedProcessor ParliamentOrgThresholdChangedProcessor;
    protected readonly ParliamentOrgWhiteListChangedProcessor ParliamentOrgWhiteListChangedProcessor;
    protected readonly ParliamentProposalReleasedProcessor ParliamentProposalReleasedProcessor;
    protected readonly ParliamentReceiptCreatedProcessor ParliamentReceiptCreatedProcessor;
    protected readonly ReferendumOrgCreatedProcessor ReferendumOrgCreatedProcessor;
    protected readonly ReferendumOrgThresholdChangedProcessor ReferendumOrgThresholdChangedProcessor;
    protected readonly ReferendumOrgWhiteListChangedProcessor ReferendumOrgWhiteListChangedProcessor;
    protected readonly ReferendumProposalReleasedProcessor ReferendumProposalReleasedProcessor;
    protected readonly ReferendumReceiptCreatedProcessor ReferendumReceiptCreatedProcessor;

    // repository
    protected readonly IReadOnlyRepository<VoteSchemeIndex> VoteSchemeIndexRepository;
    protected readonly IReadOnlyRepository<VoteItemIndex> VoteItemIndexRepository;
    protected readonly IReadOnlyRepository<VoteWithdrawnIndex> VoteWithdrawnRepository;
    protected readonly IReadOnlyRepository<DAOIndex> DAOIndexRepository;
    protected readonly IReadOnlyRepository<OrganizationIndex> organizationIndexRepository;
    protected readonly IReadOnlyRepository<LatestParticipatedIndex> LatestParticipatedIndexRepository;
    protected readonly IReadOnlyRepository<TreasuryFundIndex> TreasuryFundRepository;
    protected readonly IReadOnlyRepository<TreasuryFundSumIndex> TreasuryFundSumRepository;
    protected readonly IReadOnlyRepository<TreasuryRecordIndex> TreasuryRecordRepository;
    protected readonly IReadOnlyRepository<ElectionIndex> ElectionRepository;
    protected readonly IReadOnlyRepository<ElectionHighCouncilConfigIndex> ElectionHighCouncilConfigRepository;
    protected readonly IReadOnlyRepository<ElectionVotingItemIndex> ElectionVotingItemRepository;
    protected readonly IReadOnlyRepository<GovernanceSchemeIndex> GovernanceSchemeRepository;
    protected readonly IReadOnlyRepository<ElectionCandidateElectedIndex> CandidateElectedRepository;
    protected readonly IReadOnlyRepository<ProposalIndex> ProposalIndexRepository;
    protected readonly IReadOnlyRepository<VoteRecordIndex> VoteRecordIndexRepository;
    protected readonly IReadOnlyRepository<DaoVoterRecordIndex> DaoVoterRecordIndexRepository;
    protected readonly IReadOnlyRepository<UserBalanceIndex> UserBalanceIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoOrgChangedIndex> NetworkDaoOrgChangedIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoOrgCreatedIndex> NetworkDaoOrgCreatedIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoOrgWhiteListChangedIndex>
        NetworkDaoOrgWhiteListChangedIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoOrgThresholdChangedIndex>
        NetworkDaoOrgThresholdChangedIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoOrgMemberChangedIndex> NetworkDaoOrgMemberChangedIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoProposalIndex> NetworkDaoProposalIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoProposalReleasedIndex> NetworkDaoProposalReleasedIndexRepository;
    protected readonly IReadOnlyRepository<NetworkDaoProposalVoteRecordIndex>
        NetworkDaoProposalVoteRecordIndexRepository;

    // mapper
    protected readonly IObjectMapper ObjectMapper;

    // param
    protected static readonly string Id1 = "123";
    protected static readonly string Id2 = "456";
    protected static readonly string Id3 = "789";
    protected static readonly string Id4 = "101";
    protected static readonly string ProposalId = "p-1";
    protected readonly string DAOId = HashHelper.ComputeFrom(Id1).ToHex();
    protected readonly string VoteSchemeId = HashHelper.ComputeFrom(Id3).ToHex();
    protected readonly string VetoProposalId = HashHelper.ComputeFrom(Id4).ToHex();
    protected readonly string DAOName = "DAOName";
    protected readonly string DAOLogoUrl = "DAOLogoUrl";
    protected readonly string DAODescription = "DAODescription";
    protected readonly string DAOSocialMedia = "{ \"name\": \"url\" }";
    protected readonly string DAOMetadataAdmin = "2N9DJYUUruS7bFqRyKMvafA75qTWgqpWcB78nNZzpmxHrMv4D";
    protected readonly string DAO = "2N9DJYUUruS7bFqRyKMvafA75qTWgqpWcB78nNZzpmxHrMv4D";
    protected readonly string Elf = "ELF";
    protected readonly string tDVW = "tDVW";
    protected readonly string tDVV = "tDVV";
    protected readonly string TOMORROWPASS = "TOMORROWPASS-1";
    protected readonly string TOMORROWPASSTEST = "TOMORROWPASSTEST-1";
    protected readonly string SchemeId = HashHelper.ComputeFrom(Id2).ToHex();
    protected static readonly string SubId = "456-1";
    protected readonly string FileHash = "FileHash";
    protected readonly string FileCid = "FileCid";
    protected readonly string FileName = "FileName";
    protected readonly string FileUrl = "FileUrl";
    protected readonly string DAOCreator = "2fbCtXNLVD2SC4AD6b8nqAkHtjqxRCfwvciX4MyH6257n8Gf63";
    protected readonly string Creator = "2fbCtXNLVD2SC4AD6b8nqAkHtjqxRCfwvciX4MyH6257n8Gf63";
    protected readonly string User = "2XXstcuHsCzwaYruJA1MdsXxkposUBr2gA8ubRjqqmUyRyBM2t";
    protected readonly string OrganizationAddress = "UE6mcinaCFJZmGNgY9fpMnyzwMETJUhqwbnvtjRgX1f12rBQj";
    protected readonly string ExecuteAddress = "aLyxCJvWMQH6UEykTyeWAcYss9baPyXkrMQ37BHnUicxD2LL3";
    protected readonly string ExecuteAddressNew = "nH1x6TMcS2f2gRUTUgUGWmcBdwnXJipeZTgNXb2dxPkPaBd4M";
    protected readonly string ExecuteContractAddress = "YeCqKprLBGbZZeRTkN1FaBLXsetY8QFotmVKqo98w9K6jK2PY";
    protected readonly string ProposalDescription = HashHelper.ComputeFrom("ProposalDescription").ToHex();
    protected readonly string TreasuryContractAddress = "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX";
    protected readonly string VoteContractAddress = "vQfjcuW3RbGmkcL74YY4q3BX9UcH5rmwLmbQi3PsZxg8vE9Uk";
    protected readonly string ElectionContractAddress = "YeCqKprLBGbZZeRTkN1FaBLXsetY8QFotmVKqo98w9K6jK2PY";
    protected readonly string GovernanceContractAddress = "HJfhXPPL3Eb2wYPAc6ePmirenNzqGBAsynyeYF9tKSV2kHTAF";
    protected readonly string TimelockContractAddress = "7VzrKvnFRjrK4duJz8HNA1nWf2AJcxwwGXzTtC4MC3tKUtdbH";
    protected readonly string TreasuryAccountAddress = "pykr77ft9UUKJZLVq15wCH8PinBSjVRQ12sD1Ayq92mKFsJ1i";
    protected readonly string SchemeAddress = "2RjBxiiMKnEe72w5R6CtbdH3M8UQSmh7MfRPs7wJTNMU3KgUpm";
    protected readonly string ForumUrl = "ForumUrl";
    protected readonly string ProposalTitle = "ProposalTitle";

    // protected readonly int MinimalRequiredThreshold = 1;
    // protected readonly int MinimalVoteThreshold = 2;
    // protected readonly int MinimalApproveThreshold = 3;
    // protected readonly int MaximalAbstentionThreshold = 4;
    // protected readonly int MaximalRejectionThreshold = 5;
    protected readonly int MaxHighCouncilCandidateCount = 1;
    protected readonly int MaxHighCouncilMemberCount = 2;
    protected readonly int ElectionPeriod = 3;
    public const int MinActiveTimePeriod = 7; // days
    public const int MaxActiveTimePeriod = 15; // days
    public const int MinPendingTimePeriod = 5; // days
    public const int MaxPendingTimePeriod = 7; // days
    public const int MinExecuteTimePeriod = 3; // days
    public const int MaxExecuteTimePeriod = 5; // days
    public const int MinVetoActiveTimePeriod = 3; // days
    public const int MaxVetoActiveTimePeriod = 5; // days
    public const int MinVetoExecuteTimePeriod = 1; // days
    public const int MaxVetoExecuteTimePeriod = 3; // days

    public TomorrowDAOIndexerTestBase()
    {
        LazyServiceProvider = GetRequiredService<IAbpLazyServiceProvider>();
        FileInfosRemovedProcessor = GetRequiredService<FileInfosRemovedProcessor>();
        FileInfosUploadedProcessor = GetRequiredService<FileInfosUploadedProcessor>();
        MemberAddedProcessor = GetRequiredService<MemberAddedProcessor>();
        MemberRemovedProcessor = GetRequiredService<MemberRemovedProcessor>();
        HighCouncilDisabledProcessor = GetRequiredService<HighCouncilDisabledProcessor>();
        HighCouncilEnabledProcessor = GetRequiredService<HighCouncilEnabledProcessor>();
        SubsistStatusSetProcessor = GetRequiredService<SubsistStatusSetProcessor>();
        VoteSchemeCreatedProcessor = GetRequiredService<Vote.VoteSchemeCreatedProcessor>();
        VotingItemRegisteredProcessor = GetRequiredService<Vote.VotingItemRegisteredProcessor>();
        VoteWithdrawnProcessor = GetRequiredService<Vote.VoteWithdrawnProcessor>();
        DAOCreatedProcessor = GetRequiredService<DAOCreatedProcessor>();
        MetadataUpdatedProcessor = GetRequiredService<MetadataUpdatedProcessor>();
        TransferredProcessor = GetRequiredService<TransferredProcessor>();
        IssuedProcessor = GetRequiredService<IssuedProcessor>();
        BurnedProcessor = GetRequiredService<BurnedProcessor>();
        TreasuryCreatedProcessor = GetRequiredService<TreasuryCreatedProcessor>();
        TreasuryTransferredProcessor = GetRequiredService<TreasuryTransferredProcessor>();
        CandidateAddedProcessor = GetRequiredService<CandidateAddedProcessor>();
        CandidateAddressReplacedProcessor = GetRequiredService<CandidateAddressReplacedProcessor>();
        CandidateInfoUpdatedProcessor = GetRequiredService<CandidateInfoUpdatedProcessor>();
        CandidateRemovedProcessor = GetRequiredService<CandidateRemovedProcessor>();
        VotedProcessor = GetRequiredService<VotedProcessor>();
        VoteVotedProcessor = GetRequiredService<Vote.VotedProcessor>();
        ElectionVotingEventRegisteredProcessor = GetRequiredService<ElectionVotingEventRegisteredProcessor>();
        HighCouncilAddedProcessor = GetRequiredService<HighCouncilAddedProcessor>();
        HighCouncilRemovedProcessor = GetRequiredService<HighCouncilRemovedProcessor>();
        CandidateElectedProcessor = GetRequiredService<CandidateElectedProcessor>();
        GovernanceSchemeAddedProcessor = GetRequiredService<GovernanceSchemeAddedProcessor>();
        GovernanceSchemeThresholdRemovedProcessor = GetRequiredService<GovernanceSchemeThresholdRemovedProcessor>();
        GovernanceSchemeThresholdUpdatedProcessor = GetRequiredService<GovernanceSchemeThresholdUpdatedProcessor>();
        GovernanceTokenSetProcessor = GetRequiredService<GovernanceTokenSetProcessor>();
        ProposalCreatedProcessor = GetRequiredService<ProposalCreatedProcessor>();
        DAOProposalTimePeriodSetProcessor = GetRequiredService<DAOProposalTimePeriodSetProcessor>();
        ProposalExecutedProcessor = GetRequiredService<ProposalExecutedProcessor>();
        ProposalVetoedProcessor = GetRequiredService<ProposalVetoedProcessor>();
        ParliamentProposalCreatedProcessor = GetRequiredService<ParliamentProposalCreatedProcessor>();
        AssociationProposalCreatedProcessor = GetRequiredService<AssociationProposalCreatedProcessor>();
        ReferendumProposalCreatedProcessor = GetRequiredService<ReferendumProposalCreatedProcessor>();
        AssociationOrgCreatedProcessor = GetRequiredService<AssociationOrgCreatedProcessor>();
        AssociationOrgMemberAddedProcessor = GetRequiredService<AssociationOrgMemberAddedProcessor>();
        AssociationOrgMemberChangedProcessor = GetRequiredService<AssociationOrgMemberChangedProcessor>();
        AssociationOrgMemberRemovedProcessor = GetRequiredService<AssociationOrgMemberRemovedProcessor>();
        AssociationOrgThresholdChangedProcessor = GetRequiredService<AssociationOrgThresholdChangedProcessor>();
        AssociationOrgWhiteListChangedProcessor = GetRequiredService<AssociationOrgWhiteListChangedProcessor>();
        AssociationProposalReleasedProcessor = GetRequiredService<AssociationProposalReleasedProcessor>();
        AssociationReceiptCreatedProcessor = GetRequiredService<AssociationReceiptCreatedProcessor>();
        ParliamentOrgCreatedProcessor = GetRequiredService<ParliamentOrgCreatedProcessor>();
        ParliamentOrgThresholdChangedProcessor = GetRequiredService<ParliamentOrgThresholdChangedProcessor>();
        ParliamentOrgWhiteListChangedProcessor = GetRequiredService<ParliamentOrgWhiteListChangedProcessor>();
        ParliamentProposalReleasedProcessor = GetRequiredService<ParliamentProposalReleasedProcessor>();
        ParliamentReceiptCreatedProcessor = GetRequiredService<ParliamentReceiptCreatedProcessor>();
        ReferendumOrgCreatedProcessor = GetRequiredService<ReferendumOrgCreatedProcessor>();
        ReferendumOrgThresholdChangedProcessor = GetRequiredService<ReferendumOrgThresholdChangedProcessor>();
        ReferendumOrgWhiteListChangedProcessor = GetRequiredService<ReferendumOrgWhiteListChangedProcessor>();
        ReferendumProposalReleasedProcessor = GetRequiredService<ReferendumProposalReleasedProcessor>();
        ReferendumReceiptCreatedProcessor = GetRequiredService<ReferendumReceiptCreatedProcessor>();

        VoteSchemeIndexRepository = GetRequiredService<IReadOnlyRepository<VoteSchemeIndex>>();
        VoteItemIndexRepository = GetRequiredService<IReadOnlyRepository<VoteItemIndex>>();
        VoteWithdrawnRepository = GetRequiredService<IReadOnlyRepository<VoteWithdrawnIndex>>();
        DAOIndexRepository = GetRequiredService<IReadOnlyRepository<DAOIndex>>();
        organizationIndexRepository = GetRequiredService<IReadOnlyRepository<OrganizationIndex>>();
        LatestParticipatedIndexRepository = GetRequiredService<IReadOnlyRepository<LatestParticipatedIndex>>();
        TreasuryFundRepository = GetRequiredService<IReadOnlyRepository<TreasuryFundIndex>>();
        TreasuryFundSumRepository = GetRequiredService<IReadOnlyRepository<TreasuryFundSumIndex>>();
        TreasuryRecordRepository = GetRequiredService<IReadOnlyRepository<TreasuryRecordIndex>>();
        ElectionRepository = GetRequiredService<IReadOnlyRepository<ElectionIndex>>();
        ElectionHighCouncilConfigRepository = GetRequiredService<IReadOnlyRepository<ElectionHighCouncilConfigIndex>>();
        ElectionVotingItemRepository = GetRequiredService<IReadOnlyRepository<ElectionVotingItemIndex>>();
        GovernanceSchemeRepository = GetRequiredService<IReadOnlyRepository<GovernanceSchemeIndex>>();
        CandidateElectedRepository = GetRequiredService<IReadOnlyRepository<ElectionCandidateElectedIndex>>();
        ProposalIndexRepository = GetRequiredService<IReadOnlyRepository<ProposalIndex>>();
        VoteRecordIndexRepository = GetRequiredService<IReadOnlyRepository<VoteRecordIndex>>();
        DAOIndexRepository = GetRequiredService<IReadOnlyRepository<DAOIndex>>();
        DaoVoterRecordIndexRepository = GetRequiredService<IReadOnlyRepository<DaoVoterRecordIndex>>();
        UserBalanceIndexRepository = GetRequiredService<IReadOnlyRepository<UserBalanceIndex>>();
        NetworkDaoOrgChangedIndexRepository = GetRequiredService<IReadOnlyRepository<NetworkDaoOrgChangedIndex>>();
        NetworkDaoOrgCreatedIndexRepository = GetRequiredService<IReadOnlyRepository<NetworkDaoOrgCreatedIndex>>();
        NetworkDaoOrgThresholdChangedIndexRepository =
            GetRequiredService<IReadOnlyRepository<NetworkDaoOrgThresholdChangedIndex>>();
        NetworkDaoOrgWhiteListChangedIndexRepository =
            GetRequiredService<IReadOnlyRepository<NetworkDaoOrgWhiteListChangedIndex>>();
        NetworkDaoOrgMemberChangedIndexRepository =
            GetRequiredService<IReadOnlyRepository<NetworkDaoOrgMemberChangedIndex>>();
        NetworkDaoProposalIndexRepository = GetRequiredService<IReadOnlyRepository<NetworkDaoProposalIndex>>();
        NetworkDaoProposalReleasedIndexRepository =
            GetRequiredService<IReadOnlyRepository<NetworkDaoProposalReleasedIndex>>();
        NetworkDaoProposalVoteRecordIndexRepository =
            GetRequiredService<IReadOnlyRepository<NetworkDaoProposalVoteRecordIndex>>();

        ObjectMapper = GetRequiredService<IObjectMapper>();
    }

    protected async Task MockEventProcess<TEvent>(TEvent logEvent, LogEventProcessorBase<TEvent> processor)
        where TEvent : IEvent<TEvent>, new()
    {
        await processor.ProcessAsync(logEvent, GenerateLogEventContext(logEvent));
    }

    protected LogEventContext GenerateLogEventContext<T>(string chainId, T eventData, Transaction transaction = null)
        where T : IEvent<T>
    {
        var context = GenerateLogEventContext(eventData);
        context.ChainId = chainId;
        return context;
    }

    protected async Task CheckFileInfo(DAOIndex index)
    {
        index.ShouldNotBeNull();
        var fileInfoListString = index.FileInfoList;
        fileInfoListString.ShouldNotBeNull();
        var fileList = JsonConvert.DeserializeObject<List<FileInfoIndexer>>(fileInfoListString);
        fileList.ShouldNotBeNull();
        fileList.Count.ShouldBe(1);
        fileList[0].Uploader.ShouldBe(DAOCreator);
    }

    protected async Task<TEntity> GetIndexById<TEntity>(string id)
    {
        return await LazyServiceProvider.GetRequiredService<IRepository<TEntity>>().GetAsync(id);
    }

    protected DAOCreated MaxInfoDAOCreated()
    {
        return new DAOCreated
        {
            Metadata = new TomorrowDAO.Contracts.DAO.Metadata
            {
                Name = DAOName,
                LogoUrl = DAOLogoUrl,
                Description = DAODescription,
                SocialMedia = { ["name"] = "url" }
            },
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
            GovernanceMechanism = GovernanceMechanism.Organization
        };
    }

    protected DAOCreated MinInfoDAOCreated()
    {
        return new DAOCreated
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        };
    }

    protected FileInfosRemoved FileInfosRemoved()
    {
        return new FileInfosRemoved
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            RemovedFiles = GetFileInfoList()
        };
    }

    protected FileInfosUploaded FileInfosUploaded()
    {
        return new FileInfosUploaded
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            UploadedFiles = GetFileInfoList()
        };
    }

    protected FileInfoList GetFileInfoList()
    {
        return new FileInfoList
        {
            Data =
            {
                [FileCid] = new TomorrowDAO.Contracts.DAO.FileInfo
                {
                    File = new TomorrowDAO.Contracts.DAO.File { Cid = FileCid, Name = FileName, Url = FileUrl },
                    UploadTime = new Timestamp(),
                    Uploader = Address.FromBase58(DAOCreator)
                }
            }
        };
    }

    protected TreasuryCreated TreasuryCreated()
    {
        return new TreasuryCreated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            TreasuryAccountAddress = Address.FromBase58(TreasuryAccountAddress)
        };
    }

    protected Transferred TokenTransferred(string symbol = "ELF")
    {
        return new Transferred
        {
            From = Address.FromBase58(ExecuteAddress),
            To = Address.FromBase58(TreasuryAccountAddress),
            Symbol = symbol,
            Amount = 100000000,
            Memo = "Test",
        };
    }
    
    protected Issued Issued(string symbol = "Elf")
    {
        return new Issued
        {
            To = Address.FromBase58(ExecuteAddress),
            Symbol = symbol,
            Amount = 200000000,
            Memo = "Test",
        };
    }
    
    protected Burned Burned(string symbol = "Elf")
    {
        return new Burned
        {
            Burner = Address.FromBase58(ExecuteAddress),
            Symbol = symbol,
            Amount = 100000000
        };
    }

    protected TreasuryTransferred TreasuryTransferred()
    {
        return new TreasuryTransferred
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            TreasuryAddress = Address.FromBase58(TreasuryAccountAddress),
            Amount = 1L,
            Recipient = Address.FromBase58(DAOCreator),
            Memo = "Test",
            Executor = Address.FromBase58(ExecuteAddress),
            ProposalId = HashHelper.ComputeFrom(Id2),
            Symbol = Elf
        };
    }

    protected CandidateAdded CandidateAdded()
    {
        return new CandidateAdded
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            Candidate = Address.FromBase58(DAOCreator)
        };
    }

    protected ElectionVotingEventRegistered ElectionVotingEventRegistered()
    {
        return new ElectionVotingEventRegistered
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            Config = new TomorrowDAO.Contracts.Election.HighCouncilConfig
            {
                MaxHighCouncilMemberCount = 100,
                MaxHighCouncilCandidateCount = 200,
                ElectionPeriod = 100,
                IsRequireHighCouncilForExecution = false,
                GovernanceToken = "ELF",
                StakeThreshold = 10000
            },
            VotingItem = new VotingItem
            {
                VotingItemId = HashHelper.ComputeFrom(Id1),
                AcceptedCurrency = "ELF",
                IsLockToken = true,
                CurrentSnapshotNumber = 1,
                TotalSnapshotNumber = long.MaxValue,
                RegisterTimestamp = new Timestamp(),
                StartTimestamp = new Timestamp(),
                EndTimestamp = new Timestamp(),
                CurrentSnapshotStartTimestamp = new Timestamp(),
                Sponsor = Address.FromBase58(DAOCreator),
                IsQuadratic = false,
                TicketCost = 0
            }
        };
    }

    protected HighCouncilAdded HighCouncilAdded()
    {
        return new HighCouncilAdded
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            AddHighCouncils = new TomorrowDAO.Contracts.Election.AddressList
            {
                Value = { Address.FromBase58(Creator), Address.FromBase58(User) }
            }
        };
    }

    protected HighCouncilRemoved HighCouncilRemoved()
    {
        return new HighCouncilRemoved
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            RemoveHighCouncils = new TomorrowDAO.Contracts.Election.AddressList
            {
                Value =
                {
                    Address.FromBase58(OrganizationAddress),
                    Address.FromBase58(User)
                }
            }
        };
    }

    protected CandidateElected CandidateElected()
    {
        return new CandidateElected
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            PreTermNumber = 1,
            NewNumber = 2
        };
    }

    protected CandidateAddressReplaced CandidateAddressReplaced()
    {
        return new CandidateAddressReplaced
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            NewAddress = Address.FromBase58(User),
            OldAddress = Address.FromBase58(Creator)
        };
    }

    protected CandidateInfoUpdated CandidateInfoUpdated()
    {
        return new CandidateInfoUpdated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            CandidateAddress = Address.FromBase58(DAOCreator),
            IsEvilNode = true
        };
    }

    protected CandidateRemoved CandidateRemoved()
    {
        return new CandidateRemoved
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            Candidate = Address.FromBase58(DAOCreator)
        };
    }

    protected Voted Voted()
    {
        return new Voted
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            CandidateAddress = Address.FromBase58(DAOCreator),
            Amount = 100,
            EndTimestamp = null,
            VoteId = HashHelper.ComputeFrom(Id2)
        };
    }

    protected VoteSchemeCreated VoteSchemeCreated_UniqueVote()
    {
        return new VoteSchemeCreated
        {
            VoteMechanism = VoteMechanism.UniqueVote,
            VoteSchemeId = HashHelper.ComputeFrom(Id3),
            VoteStrategy = VoteStrategy.ProposalDistinct,
            WithoutLockToken = true
        };
    }

    protected Withdrawn VoteWithdrawn()
    {
        var votingItemIdList = new VotingItemIdList
        {
            Value = { HashHelper.ComputeFrom(VetoProposalId) }
        };
        return new Withdrawn
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            Withdrawer = Address.FromBase58(User),
            WithdrawAmount = 10,
            WithdrawTimestamp = new Timestamp(),
            VotingItemIdList = votingItemIdList
        };
    }

    protected TomorrowDAO.Contracts.Vote.Voted VoteVoted()
    {
        return new TomorrowDAO.Contracts.Vote.Voted
        {
            VotingItemId = HashHelper.ComputeFrom(Id2),
            Voter = Address.FromBase58(User),
            Amount = 100,
            VoteTimestamp = DateTime.UtcNow.AddMinutes(1).ToTimestamp(),
            Option = VoteOption.Approved,
            VoteId = HashHelper.ComputeFrom(Id3),
            DaoId = HashHelper.ComputeFrom(Id1),
            VoteMechanism = VoteMechanism.TokenBallot,
            StartTime = DateTime.UtcNow.AddMinutes(1).ToTimestamp(),
            EndTime = DateTime.UtcNow.AddMinutes(200).ToTimestamp(),
            Memo = "Memo"
        };
    }

    protected VotingItemRegistered VotingItemRegistered()
    {
        return new VotingItemRegistered
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            VotingItemId = HashHelper.ComputeFrom(Id2),
            SchemeId = HashHelper.ComputeFrom(Id4),
            AcceptedCurrency = Elf,
            RegisterTimestamp = DateTime.UtcNow.AddMinutes(-10).ToTimestamp(),
            StartTimestamp = DateTime.UtcNow.AddMinutes(-10).ToTimestamp(),
            EndTimestamp = DateTime.UtcNow.AddMinutes(100).ToTimestamp()
        };
    }

    protected VoteSchemeCreated VoteSchemeCreated_TokenBallot()
    {
        return new VoteSchemeCreated
        {
            // IsLockToken = true,
            // IsQuadratic = true,
            VoteMechanism = VoteMechanism.TokenBallot,
            VoteSchemeId = HashHelper.ComputeFrom(Id1)
        };
    }

    protected GovernanceSchemeAdded GovernanceSchemeAdded(
        TomorrowDAO.Contracts.Governance.GovernanceMechanism governanceMechanism
            = TomorrowDAO.Contracts.Governance.GovernanceMechanism.Referendum)
    {
        return new GovernanceSchemeAdded
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            GovernanceMechanism = governanceMechanism,
            SchemeThreshold = new TomorrowDAO.Contracts.Governance.GovernanceSchemeThreshold
            {
                MinimalRequiredThreshold = 10,
                MinimalVoteThreshold = 12,
                MinimalApproveThreshold = 50,
                MaximalRejectionThreshold = 30,
                MaximalAbstentionThreshold = 20,
                ProposalThreshold = 10
            },
            GovernanceToken = Elf,
            SchemeId = HashHelper.ComputeFrom(Id2),
            SchemeAddress = Address.FromBase58(SchemeAddress)
        };
    }

    protected GovernanceSchemeThresholdRemoved GovernanceSchemeThresholdRemoved()
    {
        return new GovernanceSchemeThresholdRemoved
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            SchemeAddress = Address.FromBase58(SchemeAddress)
        };
    }

    protected GovernanceSchemeThresholdUpdated GovernanceSchemeThresholdUpdated()
    {
        return new GovernanceSchemeThresholdUpdated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            UpdateSchemeThreshold = new TomorrowDAO.Contracts.Governance.GovernanceSchemeThreshold
            {
                MinimalRequiredThreshold = 1,
                MinimalVoteThreshold = 1,
                MinimalApproveThreshold = 1,
                MaximalRejectionThreshold = 1,
                MaximalAbstentionThreshold = 1
            },
            SchemeAddress = Address.FromBase58(SchemeAddress)
        };
    }

    protected GovernanceTokenSet GovernanceTokenSet()
    {
        return new GovernanceTokenSet
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            GovernanceToken = "USDT"
        };
    }

    protected ProposalCreated ProposalCreated()
    {
        return new ProposalCreated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            ProposalId = HashHelper.ComputeFrom(ProposalId),
            ProposalTitle = ProposalTitle,
            ForumUrl = ForumUrl,
            ProposalDescription = ProposalDescription,
            ProposalType = ProposalType.Advisory,
            ActiveStartTime = new Timestamp(),
            ActiveEndTime = new Timestamp(),
            ExecuteStartTime = new Timestamp(),
            ExecuteEndTime = new Timestamp(),
            ProposalStatus = ProposalStatus.Empty,
            ProposalStage = ProposalStage.Active,
            Proposer = Address.FromBase58(DAOCreator),
            SchemeAddress = Address.FromBase58(SchemeAddress),
            Transaction = new TomorrowDAO.Contracts.Governance.ExecuteTransaction
            {
                ContractMethodName = "ContractMethodName",
                ToAddress = Address.FromBase58(ExecuteAddress),
                Params = ByteStringHelper.FromHexString("0102030405")
            },
            VoteSchemeId = HashHelper.ComputeFrom(Id3),
            VetoProposalId = HashHelper.ComputeFrom(Id4)
        };
    }

    protected ProposalCreated ProposalCreated_Veto()
    {
        return new ProposalCreated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            ProposalId = HashHelper.ComputeFrom(Id4)
        };
    }

    protected DaoProposalTimePeriodSet DaoProposalTimePeriodSet()
    {
        return new DaoProposalTimePeriodSet
        {
            ActiveTimePeriod = 1L,
            DaoId = HashHelper.ComputeFrom(Id1),
            ExecuteTimePeriod = 2L,
            PendingTimePeriod = 3L,
            VetoExecuteTimePeriod = 4L,
            VetoActiveTimePeriod = 5L
        };
    }

    protected ProposalExecuted ProposalExecuted(string proposalId = null)
    {
        return new ProposalExecuted
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            ExecuteTime = new Timestamp(),
            ProposalId = HashHelper.ComputeFrom(proposalId ?? ProposalId)
        };
    }

    protected ProposalVetoed ProposalVetoed()
    {
        return new ProposalVetoed
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            VetoProposalId = HashHelper.ComputeFrom(ProposalId),
            ProposalId = HashHelper.ComputeFrom(Id4),
            VetoTime = new Timestamp()
        };
    }

    protected MetadataUpdated MetadataUpdated()
    {
        return new MetadataUpdated
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            Metadata = new TomorrowDAO.Contracts.DAO.Metadata
            {
                Name = "update",
                LogoUrl = "update",
                Description = "update",
                SocialMedia = { ["update"] = "update" }
            }
        };
    }

    protected MemberAdded MemberAdded()
    {
        return new MemberAdded
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            AddMembers = new AddressListDAO
            {
                Value = { Address.FromBase58(User), Address.FromBase58(Creator) }
            }
        };
    }

    protected MemberRemoved MemberRemoved()
    {
        return new MemberRemoved
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            RemoveMembers = new AddressListDAO
            {
                Value = { Address.FromBase58(User), Address.FromBase58(DAOCreator), Address.FromBase58(Creator) }
            }
        };
    }

    protected AElf.Standards.ACS3.ProposalCreated NetworkDaoProposalCreate(string proposalId = null)
    {
        return new AElf.Standards.ACS3.ProposalCreated
        {
            ProposalId = HashHelper.ComputeFrom(proposalId ?? ProposalId),
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            Title = ProposalTitle,
            Description = ProposalDescription
        };
    }

    protected AElf.Standards.ACS3.OrganizationCreated NetworkDaoOrganizationCreated(string organizationAddress = null)
    {
        return new AElf.Standards.ACS3.OrganizationCreated
        {
            OrganizationAddress = Address.FromBase58(organizationAddress ?? OrganizationAddress)
        };
    }

    protected AElf.Contracts.Association.MemberAdded NetworkDaoOrgMemberAdded()
    {
        return new AElf.Contracts.Association.MemberAdded
        {
            Member = Address.FromBase58(User),
            OrganizationAddress = Address.FromBase58(OrganizationAddress)
        };
    }

    protected AElf.Contracts.Association.MemberRemoved NetworkDaoOrgMemberRemoved()
    {
        return new AElf.Contracts.Association.MemberRemoved
        {
            Member = Address.FromBase58(User),
            OrganizationAddress = Address.FromBase58(OrganizationAddress)
        };
    }

    protected AElf.Contracts.Association.MemberChanged NetworkDaoOrgMemberChanged()
    {
        return new AElf.Contracts.Association.MemberChanged
        {
            OldMember = Address.FromBase58(User),
            NewMember = Address.FromBase58(Creator),
            OrganizationAddress = Address.FromBase58(OrganizationAddress)
        };
    }

    protected AElf.Standards.ACS3.OrganizationThresholdChanged NetworkDaoOrgThresholdChanged(
        string organizationAddress = null)
    {
        return new AElf.Standards.ACS3.OrganizationThresholdChanged
        {
            OrganizationAddress = Address.FromBase58(organizationAddress ?? OrganizationAddress),
            ProposerReleaseThreshold = new ProposalReleaseThreshold
            {
                MinimalApprovalThreshold = 10,
                MaximalRejectionThreshold = 10,
                MaximalAbstentionThreshold = 10,
                MinimalVoteThreshold = 10
            }
        };
    }

    protected AElf.Standards.ACS3.OrganizationWhiteListChanged NetworkDaoOrgWhiteListChanged(
        string organizationAddress = null)
    {
        return new AElf.Standards.ACS3.OrganizationWhiteListChanged
        {
            OrganizationAddress = Address.FromBase58(organizationAddress ?? OrganizationAddress),
            ProposerWhiteList = new ProposerWhiteList
            {
                Proposers = { Address.FromBase58(User), Address.FromBase58(Creator) }
            }
        };
    }

    protected AElf.Standards.ACS3.ProposalReleased NetworkDaoProposalReleased()
    {
        return new AElf.Standards.ACS3.ProposalReleased
        {
            ProposalId = HashHelper.ComputeFrom(ProposalId),
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
            Title = ProposalTitle,
            Description = ProposalDescription,
        };
    }

    protected AElf.Standards.ACS3.ReceiptCreated NetworkDaoProposalReceiptCreated(
        ReceiptTypeEnum receiptTypeEnum = ReceiptTypeEnum.Approve)
    {
        return new AElf.Standards.ACS3.ReceiptCreated
        {
            ProposalId = HashHelper.ComputeFrom(ProposalId),
            Address = Address.FromBase58(User),
            ReceiptType = receiptTypeEnum.ToString(),
            Time = new Timestamp(),
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
        };
    }

    protected ReferendumReceiptCreated NetworkDaoProposalReferendumReceiptCreated(
        ReceiptTypeEnum receiptTypeEnum = ReceiptTypeEnum.Approve)
    {
        return new ReferendumReceiptCreated
        {
            ProposalId = HashHelper.ComputeFrom(ProposalId),
            Address = Address.FromBase58(User),
            Symbol = Elf,
            Amount = 100,
            ReceiptType = receiptTypeEnum.ToString(),
            Time = new Timestamp(),
            OrganizationAddress = Address.FromBase58(OrganizationAddress),
        };
    }
}