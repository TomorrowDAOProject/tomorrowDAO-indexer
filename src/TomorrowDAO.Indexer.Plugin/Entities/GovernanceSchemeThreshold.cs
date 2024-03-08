using AElfIndexer.Client;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class GovernanceSchemeThreshold : AElfIndexerClientEntity<string>
{
    public int MinimalRequiredThreshold { get; set; }
    
    public int MinimalVoteThreshold { get; set; }
    
    public int MinimalApproveThreshold { get; set; }
    
    public int MaximalRejectionThreshold { get; set; }
    
    public int MaximalAbstentionThreshold { get; set; }
    
    public void OfThreshold(TomorrowDAO.Contracts.Governance.GovernanceSchemeThreshold schemeThreshold)
    {
        MinimalRequiredThreshold = (int)schemeThreshold.MinimalRequiredThreshold;
        MinimalVoteThreshold = (int)schemeThreshold.MinimalVoteThreshold;
        MinimalApproveThreshold = (int)schemeThreshold.MinimalApproveThreshold;
        MaximalRejectionThreshold = (int)schemeThreshold.MaximalRejectionThreshold;
        MaximalAbstentionThreshold = (int)schemeThreshold.MaximalAbstentionThreshold;
    }
}