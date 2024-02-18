using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class VoteRecordIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    
    // The voting activity id.(proposal id/customize)
    [Keyword] public string VotingItemId { get; set; }
    
    [Keyword] public string Voter { get; set; }
    
    public int Amount { get; set; }
    
    // Approve/reject/abstain     
    public VoteOption Option { get; set; }
    
    public DateTime VoteTime { get; set; }
}
