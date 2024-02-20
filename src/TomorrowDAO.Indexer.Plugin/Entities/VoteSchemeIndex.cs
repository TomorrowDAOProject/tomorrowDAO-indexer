using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class VoteSchemeIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string VoteSchemeId { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public VoteMechanism VoteMechanism { get; set; }
    public bool IsLockToken { get; set; }
    public bool IsQuadratic { get; set; }
    public long TicketCost { get; set; }
    public DateTime CreateTime { get; set; }
}