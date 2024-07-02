using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class DAOInfoDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    public string Creator { get; set; }
    public string Name { get; set; }
    public string LogoUrl { get; set; }
    public string Description { get; set; }
    public string SocialMedia { get; set; }
    public string GovernanceToken { get; set; }
    public bool IsHighCouncilEnabled { get; set; }
    public string HighCouncilAddress { get; set; }
    public long MaxHighCouncilMemberCount { get; set; }
    public long MaxHighCouncilCandidateCount { get; set; }
    public long ElectionPeriod { get; set; }
    public long StakingAmount { get; set; }
    public long HighCouncilTermNumber { get; set; }
    public string FileInfoList { get; set; }
    public bool IsTreasuryContractNeeded { get; set; }
    public bool SubsistStatus { get; set; }
    public string TreasuryContractAddress { get; set; }
    public string TreasuryAccountAddress { get; set; }
    public bool IsTreasuryPause { get; set; }
    public string? TreasuryPauseExecutor { get; set; }
    public string VoteContractAddress { get; set; }
    public string ElectionContractAddress { get; set; }
    public string GovernanceContractAddress { get; set; }
    public string TimelockContractAddress { get; set; }
    public long ActiveTimePeriod { get; set; }
    public long VetoActiveTimePeriod { get; set; }
    public long PendingTimePeriod { get; set; }
    public long ExecuteTimePeriod { get; set; }
    public long VetoExecuteTimePeriod { get; set; }
    public DateTime CreateTime { get; set; }
    public bool IsNetworkDAO { get; set; }
    public int VoterCount { get; set; }
    public long VoteAmount { get; set; }
    public long WithdrawAmount { get; set; }
    public GovernanceMechanism GovernanceMechanism { get; set; }
}