namespace TomorrowDAOIndexer.GraphQL.Dto;

public class ElectionVotingItemDto
{
    public long TotalCount { get; set; }
    public List<ElectionVotingItem> Items { get; set; }
}

public class ElectionVotingItem : BlockInfoDto
{
    public string Id { get; set; }
    public string DaoId { get; set; }
    public string VotingItemId { get; set; }
    public string AcceptedCurrency { get; set; }
    public bool IsLockToken { get; set; }
    public long CurrentSnapshotNumber { get; set; }
    public long TotalSnapshotNumber { get; set; }
    public string Options { get; set; }
    public DateTime RegisterTimestamp { get; set; }
    public DateTime StartTimestamp { get; set; }
    public DateTime EndTimestamp { get; set; }
    public DateTime CurrentSnapshotStartTimestamp { get; set; }
    public string Sponsor { get; set; }
    public bool IsQuadratic { get; set; }
    public long TicketCost { get; set; }
}

public class GetElectionVotingItemIndexInput
{
    public string ChainId { get; set; }
    public string DaoId { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
}