using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GovernanceSchemeIndexDto
{
    public string Id { get; set; }
    public string DAOId { get; set; }
    public string SchemeId { get; set; }
    public string SchemeAddress { get; set; }
    public string ChainId { get; set; }
    public GovernanceMechanism GovernanceMechanism { get; set; }
    public string GovernanceToken { get; set; }
    public DateTime CreateTime { get; set; }
    public GovernanceSchemeThreshold SchemeThreshold { get; set; }
}

public class GovernanceSchemeThreshold
{
    public int MinimalRequiredThreshold { get; set; }
    public int MinimalVoteThreshold { get; set; }
    public int MinimalApproveThreshold { get; set; }
    public int MaximalRejectionThreshold { get; set; }
    public int MaximalAbstentionThreshold { get; set; }
}