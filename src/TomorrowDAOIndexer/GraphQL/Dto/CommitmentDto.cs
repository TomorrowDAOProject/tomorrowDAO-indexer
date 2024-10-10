namespace TomorrowDAOIndexer.GraphQL.Dto;

public class CommitmentDto
{
    public string ChainId { get; set; }
    public string? TransactionId { get; set; }
    public long BlockHeight { get; set; }
    public string DaoId { get; set; }
    public string VotingItemId { get; set; }
    public string ProposalId { get; set; }
    public string Voter { get; set; }
    public string Commitment { get; set; }
    public long LeafIndex { get; set; }
    public DateTime Timestamp { get; set; }
}