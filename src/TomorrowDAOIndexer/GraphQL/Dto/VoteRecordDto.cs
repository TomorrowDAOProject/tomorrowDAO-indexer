using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class VoteRecordDto
{
    public string? TransactionId { get; set; }
    public long BlockHeight { get; set; }
    public string ChainId { get; set; }
    public string Id { get; set; }
    public string DAOId { get; set; }
    public string VotingItemId { get; set; }
    public string Voter { get; set; }
    public VoteMechanism VoteMechanism { get; set; }
    public long Amount { get; set; }
    public VoteOption Option { get; set; }
    public DateTime VoteTime { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}