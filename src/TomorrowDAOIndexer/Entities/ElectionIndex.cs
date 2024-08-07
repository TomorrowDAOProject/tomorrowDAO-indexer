using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class ElectionIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    public long BlockHeight { get; set; }
    // [PropertyName("DAOId")]
    [Keyword] public string DAOId { get; set; }
    public long TermNumber { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public HighCouncilType HighCouncilType { get; set; }
    [Keyword] public string Address { get; set; }
    public long VotesAmount { get; set; }
    public long StakeAmount { get; set; }
}