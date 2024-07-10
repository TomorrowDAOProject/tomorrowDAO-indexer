namespace TomorrowDAOIndexer.GraphQL.Dto;

public class ElectionHighCouncilConfigDto
{
    public long TotalCount { get; set; }
    public List<ElectionHighCouncilConfig> Items { get; set; }
}

public class ElectionHighCouncilConfig : BlockInfoDto
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
}

public class GetElectionHighCouncilListInput
{
    public string ChainId { get; set; }
    public string DaoId { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
}