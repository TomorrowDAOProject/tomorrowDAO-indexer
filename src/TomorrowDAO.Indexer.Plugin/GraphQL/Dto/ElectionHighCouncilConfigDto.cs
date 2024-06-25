namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class ElectionHighCouncilConfigDto
{
    public long Count { get; set; }
    public List<ElectionHighCouncilConfig> Data { get; set; }
}

public class ElectionHighCouncilConfig
{
    public string Id { get; set; }
    public string DaoId { get; set; }
    public long MaxHighCouncilMemberCount { get; set; }
    public long MaxHighCouncilCandidateCount { get; set; }
    public long ElectionPeriod { get; set; }
    public bool IsRequireHighCouncilForExecution { get; set; }
    public string GovernanceToken { get; set; }
    public long StakeThreshold { get; set; }
    public List<string> InitialHighCouncilMembers { get; set; } = new List<string>();

    public string ChainId { get; set; }
    public string BlockHash { get; set; }
    public long BlockHeight { get; set; }
    public string PreviousBlockHash { get; set; }
    public bool IsDeleted { get; set; }
}

public class GetElectionHighCouncilListInput
{
    public string ChainId { get; set; }
    public string DaoId { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
}