using AeFinder.Sdk.Entities;
using Nest;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAOIndexer.Entities;

public class GovernanceSchemeIndex : GovernanceSchemeThreshold, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    public long BlockHeight { get; set; }
    // [PropertyName("DAOId")]
    [Keyword] public string DAOId { get; set; }
    [Keyword] public string SchemeId { get; set; }
    [Keyword] public string SchemeAddress { get; set; }
    public GovernanceMechanism GovernanceMechanism { get; set; }
    [Keyword] public string GovernanceToken { get; set; }
    public DateTime CreateTime { get; set; }
}