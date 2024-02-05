using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class GovernanceSubSchemeIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    
    [Keyword] public string ParentSchemeId { get; set; }
   
    public GovernanceMechanism GovernanceMechanism { get; set; }
   
    public int MinimalRequiredThreshold { get; set; }
        
    public int MinimalVoteThreshold { get; set; }
    //percentage            
    public int MinimalApproveThreshold { get; set; }
    //percentage    
    public int MinimalRejectionThreshold { get; set; }
    //percentage    
    public int MinimalAbstentionThreshold { get; set; }
    
    public DateTime CreateTime { get; set; }
}