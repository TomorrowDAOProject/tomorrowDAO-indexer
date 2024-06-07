using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class LatestParticipatedIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [PropertyName("DAOId")]
    [Keyword] public string DAOId { get; set; }
    [Keyword] public string Address { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public ParticipatedType ParticipatedType { get; set; }
    public DateTime LatestParticipatedTime { get; set; }
}