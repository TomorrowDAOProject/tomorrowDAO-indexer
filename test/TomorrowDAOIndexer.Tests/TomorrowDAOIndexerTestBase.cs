using AeFinder.App.TestBase;
using AeFinder.Sdk;
using AeFinder.Sdk.Processor;
using AElf;
using AElf.CSharp.Core;
using AElf.Types;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Processors.DAO;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer;

public abstract class TomorrowDAOIndexerTestBase: AeFinderAppTestBase<TomorrowDAOIndexerTestModule>
{
    protected readonly DAOCreatedProcessor _DAOCreatedProcessor;
    protected readonly IReadOnlyRepository<DAOIndex> _DAORepository;
    protected readonly IObjectMapper _objectMapper;
    
    // protected readonly long BlockHeight = 120;
    // protected readonly string ChainAelf = "tDVV";
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
        _DAOCreatedProcessor = GetRequiredService<DAOCreatedProcessor>();
        _DAORepository = GetRequiredService<IReadOnlyRepository<DAOIndex>>();
        _objectMapper = GetRequiredService<IObjectMapper>();
    }

    protected async Task MockEventProcess<TEvent>(TEvent logEvent, LogEventProcessorBase<TEvent> processor) where TEvent : IEvent<TEvent>, new()
    {
        await processor.ProcessAsync(logEvent, GenerateLogEventContext(logEvent));
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
}