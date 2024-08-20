using AeFinder.Sdk.Entities;
using Nest;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class VoteRecordIndex : AeFinderEntity, IAeFinderEntity
{
    
    [Keyword] public string TransactionId { get; set; }
    public long BlockHeight { get; set; }
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DAOId { get; set; }
    // The voting activity id.(proposal id)
    [Keyword] public string VotingItemId { get; set; }
    [Keyword] public string Voter { get; set; }
    [Keyword] public VoteMechanism VoteMechanism { get; set; }
    public long Amount { get; set; }
    public VoteOption Option { get; set; }
    public DateTime VoteTimestamp { get; set; }
    [Keyword] public string VoteId { get; set; }
    
    public bool IsFinished { get; set; }
    public bool IsWithdraw { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Memo { get; set; }
}
