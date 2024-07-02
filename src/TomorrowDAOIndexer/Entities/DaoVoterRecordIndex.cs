using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class DaoVoterRecordIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DaoId { get; set; }
    [Keyword] public string VoterAddress { get; set; }
    public int Count { get; set; }
}