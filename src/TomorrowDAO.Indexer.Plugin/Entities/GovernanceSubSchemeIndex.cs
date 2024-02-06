using AElf.Indexing.Elasticsearch;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class GovernanceSubSchemeIndex : GovernanceSchemeThreshold, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    
    [Keyword] public string SubSchemeId { get; set; }

    [Keyword] public string ParentSchemeId { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public GovernanceMechanism GovernanceMechanism { get; set; }
    
    public DateTime CreateTime { get; set; }
}