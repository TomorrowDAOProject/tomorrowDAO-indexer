using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class VoteRecordIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DAOId { get; set; }
    // The voting activity id.(proposal id)
    [Keyword] public string VotingItemId { get; set; }
    [Keyword] public string Voter { get; set; }
    [Keyword] public VoteMechanism VoteMechanism { get; set; }
    public int Amount { get; set; }
    public VoteOption Option { get; set; }
    public DateTime VoteTimestamp { get; set; }
    [Keyword] public string VoteId { get; set; }
    
    public bool IsFinished { get; set; }
    public bool IsWithdraw { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}
