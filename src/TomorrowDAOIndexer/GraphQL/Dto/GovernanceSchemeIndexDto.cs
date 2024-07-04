namespace TomorrowDAOIndexer.GraphQL.Dto;

public class GovernanceSchemeIndexDto
{
    public string Id { get; set; }
    public string DAOId { get; set; }
    public string SchemeId { get; set; }
    public string SchemeAddress { get; set; }
    public string ChainId { get; set; }
    public int GovernanceMechanism { get; set; }
    public string GovernanceToken { get; set; }
    public DateTime CreateTime { get; set; }
    public long MinimalRequiredThreshold { get; set; }
    public long MinimalVoteThreshold { get; set; }
    public long MinimalApproveThreshold { get; set; }
    public long MaximalRejectionThreshold { get; set; }
    public long MaximalAbstentionThreshold { get; set; }
    public long ProposalThreshold { get; set; }
}