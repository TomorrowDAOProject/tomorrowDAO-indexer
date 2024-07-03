using AeFinder.Sdk.Entities;
using Nest;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAOIndexer.Entities;

public class GovernanceSchemeIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [PropertyName("DAOId")]
    [Keyword] public string DAOId { get; set; }
    [Keyword] public string SchemeId { get; set; }
    [Keyword] public string SchemeAddress { get; set; }
    public GovernanceMechanism GovernanceMechanism { get; set; }
    [Keyword] public string GovernanceToken { get; set; }
    public DateTime CreateTime { get; set; }
    
    public long MinimalRequiredThreshold { get; set; }
    
    public long MinimalVoteThreshold { get; set; }
    
    public long MinimalApproveThreshold { get; set; }
    
    public long MaximalRejectionThreshold { get; set; }
    
    public long MaximalAbstentionThreshold { get; set; }
    
    public long ProposalThreshold { get; set; }
    
    public void OfThreshold(TomorrowDAO.Contracts.Governance.GovernanceSchemeThreshold schemeThreshold)
    {
        MinimalRequiredThreshold = schemeThreshold.MinimalRequiredThreshold;
        MinimalVoteThreshold = schemeThreshold.MinimalVoteThreshold;
        MinimalApproveThreshold = schemeThreshold.MinimalApproveThreshold;
        MaximalRejectionThreshold = schemeThreshold.MaximalRejectionThreshold;
        MaximalAbstentionThreshold = schemeThreshold.MaximalAbstentionThreshold;
        ProposalThreshold = schemeThreshold.ProposalThreshold;
    }
}