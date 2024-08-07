using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class ProposalSyncDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    
    public string DAOId { get; set; }

    public string ProposalId { get; set; }

    public string ProposalTitle { get; set; }
    
    public string ProposalDescription { get; set; }
    
    public string ForumUrl { get; set; }
    
    public ProposalType ProposalType { get; set; }
    
    public DateTime ActiveStartTime { get; set; }
   
    public DateTime ActiveEndTime { get; set; }
    
    public DateTime ExecuteStartTime { get; set; }

    public DateTime ExecuteEndTime { get; set; }
    
    public int ProposalStatus { get; set; }
    
    public int ProposalStage { get; set; }
    
    public string Proposer { get; set; }
    
    public string SchemeAddress { get; set; }
    
    public ExecuteTransactionDto? Transaction { get; set; }
    
    public string VoteSchemeId { get; set; }
    
    public string VetoProposalId { get; set; }
    
    public string? BeVetoProposalId { get; set; }

    public DateTime DeployTime { get; set; }

    public DateTime? ExecuteTime { get; set; }   
    
    public int GovernanceMechanism { get; set; }
    
    public long MinimalRequiredThreshold { get; set; }
    
    public long MinimalVoteThreshold { get; set; }
    
    //percentage            
    public long MinimalApproveThreshold { get; set; }
    
    //percentage    
    public long MaximalRejectionThreshold { get; set; }
    
    //percentage    
    public long MaximalAbstentionThreshold { get; set; }
    
    public long ProposalThreshold { get; set; }
    
    public long ActiveTimePeriod { get; set; }
    
    public long VetoActiveTimePeriod { get; set; }
    
    public long PendingTimePeriod { get; set; }
    
    public long ExecuteTimePeriod { get; set; }
    
    public long VetoExecuteTimePeriod { get; set; }
    
    public bool IsNetworkDAO { get; set; }
}

public class ExecuteTransactionDto
{
    // The address of the target contract.
    public string ToAddress { get; set; }

    // The method that this proposal will call when being released.
    public string ContractMethodName { get; set; }

    //key is paramName, value is param value
    public string Params { get; set; }
}