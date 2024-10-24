using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class CommitmentIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string TransactionId { get; set; }
    public long BlockHeight { get; set; }
    [Keyword] public string DaoId { get; set; }
    [Keyword] public string ProposalId { get; set; }
    [Keyword] public string Voter { get; set; }
    public string Commitment { get; set; }
    public long LeafIndex { get; set; }
    public DateTime Timestamp { get; set; }
}