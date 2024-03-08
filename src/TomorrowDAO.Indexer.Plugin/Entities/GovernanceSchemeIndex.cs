using AElf.Indexing.Elasticsearch;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class GovernanceSchemeIndex : GovernanceSchemeThreshold, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [PropertyName("DAOId")]
    [Keyword] public string DAOId { get; set; }
    [Keyword] public string SchemeId { get; set; }
    [Keyword] public string SchemeAddress { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public GovernanceMechanism GovernanceMechanism { get; set; }
    [Keyword] public string GovernanceToken { get; set; }
    public DateTime CreateTime { get; set; }
}