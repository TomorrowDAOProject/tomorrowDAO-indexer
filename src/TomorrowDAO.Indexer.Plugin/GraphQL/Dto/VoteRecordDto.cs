using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class VoteRecordDto
{
    public string Voter { get; set; }
    
    public int Amount { get; set; }
    
    // Approve/reject/abstain     
    public VoteOption Option { get; set; }
    
    public DateTime VoteTime { get; set; }
}