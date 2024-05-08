using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class ElectionVotingItemIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DaoId { get; set; }
    [Keyword] public string VotingItemId { get; set; }
    public string AcceptedCurrency { get; set; }
    public bool IsLockToken { get; set; }
    public long CurrentSnapshotNumber { get; set; }

    public long TotalSnapshotNumber { get; set; }

    // The list of options.
    public string Options { get; set; }
    public DateTime RegisterTimestamp { get; set; }
    public DateTime StartTimestamp { get; set; }
    public DateTime EndTimestamp { get; set; }
    public DateTime CurrentSnapshotStartTimestamp { get; set; }
    [Keyword] public string Sponsor { get; set; }
    public bool IsQuadratic { get; set; }
    public long TicketCost { get; set; }
}