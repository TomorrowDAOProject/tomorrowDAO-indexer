using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class OrganizationIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    
    [Keyword] public string OrganizationAddress { get; set; }
        
    [Keyword] public string OrganizationName { get; set; }
    
    public List<string> OrganizationMemberList { get; set; }
            
    //sub_scheme_id
    [Keyword] public string GovernanceSchemeId { get; set; }
    
    [Keyword] public string TokenSymbol { get; set; }
    
    public DateTime CreateTime { get; set; }
}