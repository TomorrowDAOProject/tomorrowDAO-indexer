using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class OrganizationIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    
    [Keyword] public string OrganizationAddress { get; set; }
        
    [Keyword] public string OrganizationName { get; set; }
    
    [Keyword] public HashSet<string> OrganizationMemberSet { get; set; }
            
    //sub_scheme_id
    [Keyword] public string GovernanceSchemeId { get; set; }
    
    [Keyword] public string Symbol { get; set; }
    
    public DateTime CreateTime { get; set; }
    
    public void AddMembers(HashSet<string> members)
    {
        if (members.IsNullOrEmpty())
        {
            return;
        }
        OrganizationMemberSet.UnionWith(members);
    }
    
    public void RemoveMembers(HashSet<string> members)
    {
        if (members.IsNullOrEmpty())
        {
            return;
        }
        OrganizationMemberSet.ExceptWith(members);
    }
    
    public void ChangeMember(string oldMember, string newMember)
    {
        if (newMember.IsNullOrEmpty())
        {
            return;
        }

        OrganizationMemberSet.Remove(oldMember);
        OrganizationMemberSet.Add(newMember);
    }
}