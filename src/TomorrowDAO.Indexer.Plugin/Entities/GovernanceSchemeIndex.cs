using AElf.Indexing.Elasticsearch;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class GovernanceSchemeIndex : GovernanceSchemeThreshold, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    
    [Keyword] public string GovernanceSchemeId { get; set; }
   
    [JsonConverter(typeof(StringEnumConverter))]
    public GovernanceMechanism GovernanceMechanism { get; set; }
   
    [Keyword] public string Creator { get; set; }
    
    public DateTime CreateTime { get; set; }
}