using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class VoteRecordDto
{
    public string Voter { get; set; }
    
    public int Amount { get; set; }
    
    public string TransactionId { get; set; }
    public VoteMechanism VoteMechanism { get; set; }
    
    // Approve/reject/abstain     
    public VoteOption Option { get; set; }
    public string VotingItemId { get; set; }
    
    public DateTime VoteTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}