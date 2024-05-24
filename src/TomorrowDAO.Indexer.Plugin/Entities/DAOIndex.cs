using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class DAOIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Creator { get; set; }
    public Metadata Metadata { get; set; }
    [Keyword] public string GovernanceToken { get; set; }
    public bool IsHighCouncilEnabled { get; set; }
    [Keyword] public string HighCouncilAddress { get; set; }
    public long MaxHighCouncilMemberCount { get; set; }
    public long MaxHighCouncilCandidateCount { get; set; }
    public long ElectionPeriod { get; set; }
    public long StakingAmount { get; set; }
    public long HighCouncilTermNumber { get; set; }
    [Keyword] public string FileInfoList { get; set; }
    public bool IsTreasuryContractNeeded { get; set; }
    public bool SubsistStatus { get; set; }
    [Keyword] public string TreasuryContractAddress { get; set; }
    [Keyword] public string TreasuryAccountAddress { get; set; }
    public bool IsTreasuryPause { get; set; }
    [Keyword] public string TreasuryPauseExecutor { get; set; }
    [Keyword] public string VoteContractAddress { get; set; }
    [Keyword] public string ElectionContractAddress { get; set; }
    [Keyword] public string GovernanceContractAddress { get; set; }
    [Keyword] public string TimelockContractAddress { get; set; }
    public long ActiveTimePeriod { get; set; }
    public long VetoActiveTimePeriod { get; set; }
    public long PendingTimePeriod { get; set; }
    public long ExecuteTimePeriod { get; set; }
    public long VetoExecuteTimePeriod { get; set; }
    public DateTime CreateTime { get; set; }
    public bool IsNetworkDAO { get; set; }
    //voter address Count
    public int VoterCount { get; set; }
}