using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class ElectionIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DAOId { get; set; }
    public long TermNumber { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public HighCouncilType HighCouncilType { get; set; }
    [Keyword] public string Address { get; set; }
    public bool IsRemoved { get; set; }
}