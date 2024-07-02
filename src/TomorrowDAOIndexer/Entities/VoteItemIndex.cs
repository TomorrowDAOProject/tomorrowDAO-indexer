using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class VoteItemIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }

    [PropertyName("DAOId")] [Keyword] public string DAOId { get; set; }
    [Keyword] public string Executer { get; set; }

    // The voting activity id.(proposal id)
    [Keyword] public string VotingItemId { get; set; }

    [Keyword] public string VoteSchemeId { get; set; }

    [Keyword] public string AcceptedCurrency { get; set; }

    public DateTime RegisterTime { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public long ApprovedCount { get; set; }

    public long RejectionCount { get; set; }

    public long AbstentionCount { get; set; }

    public long VotesAmount { get; set; }

    public HashSet<string> VoterSet { get; set; }

    public int VoterCount { get; set; }

    public DateTime CreateTime { get; set; }
}