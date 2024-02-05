using AElfIndexer.Client;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class ProposalBase : AElfIndexerClientEntity<string>
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string DAOId { get; set; }

    [Keyword] public string ProposalId { get; set; }

    [Keyword] public string ProposalTitle { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ProposalType ProposalType { get; set; }

    //get from GovernanceSchemeId
    [JsonConverter(typeof(StringEnumConverter))]
    public GovernanceMechanism? GovernanceMechanism { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ProposalStatus ProposalStatus { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime ExpiredTime { get; set; }
    [Keyword] public string ReleaseAddress { get; set; }

    [Keyword] public string ProposalDescription { get; set; }
    
    public CallTransactionInfo TransactionInfo { get; set; }

    //sub_scheme_id
    [Keyword] public string GovernanceSchemeId { get; set; }

    [Keyword] public string VoteSchemeId { get; set; }

    public bool ExecuteByHighCouncil { get; set; }

    public DateTime DeployTime { get; set; }    
    
    public bool VoteFinished  { get; set; } 
    
    //--------Organization info-------
    [Keyword] public string OrganizationAddress { get; set; }

    public int OrganizationMemberCount { get; set; }

    //--------Governance Threshold param-------
    public int MinimalRequiredThreshold { get; set; }
    
    public int MinimalVoteThreshold { get; set; }
    
    //percentage            
    public int MinimalApproveThreshold { get; set; }
    
    //percentage    
    public int MinimalRejectionThreshold { get; set; }
    
    //percentage    
    public int MinimalAbstentionThreshold { get; set; }
    
    //--------Vote Result-------
    public string AcceptedCurrency { get; set; }

    public int ApproveCounts { get; set; }

    public int RejectCounts { get; set; }

    public int AbstainCounts { get; set; }

    public int VotesAmount { get; set; }
    
    public int VoterCount { get; set; }
}