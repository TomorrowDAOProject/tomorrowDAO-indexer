using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class LatestParticipatedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    public long BlockHeight { get; set; }
    // [PropertyName("DAOId")]
    [Keyword] public string DAOId { get; set; }
    [Keyword] public string Address { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public ParticipatedType ParticipatedType { get; set; }
    public DateTime LatestParticipatedTime { get; set; }
}