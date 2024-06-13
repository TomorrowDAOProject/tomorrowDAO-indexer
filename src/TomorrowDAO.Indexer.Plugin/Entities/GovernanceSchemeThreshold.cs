using AElfIndexer.Client;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class GovernanceSchemeThreshold : AElfIndexerClientEntity<string>
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