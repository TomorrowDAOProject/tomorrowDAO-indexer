using AeFinder.Sdk.Entities;

namespace TomorrowDAOIndexer.Entities;

public class GovernanceSchemeThreshold : AeFinderEntity, IAeFinderEntity
{
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