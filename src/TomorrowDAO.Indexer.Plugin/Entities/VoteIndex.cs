using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class VoteIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    
    // The voting activity id.(proposal id/customize)
    [Keyword] public string VotingItemId { get; set; }
    
    [Keyword] public string VoteSchemeId { get; set; }
    
    [Keyword] public string DaoId { get; set; }
        
    [Keyword] public string AcceptedCurrency { get; set; }
   
    public int ApproveCounts  { get; set; }
    
    public int RejectCounts  { get; set; }
        
    public int AbstainCounts  { get; set; }
                
    public int VotesAmount  { get; set; }        
        
    public DateTime RegisterTime { get; set; }
    
    public DateTime StartTime { get; set; }
        
    public DateTime EndTime { get; set; }
}