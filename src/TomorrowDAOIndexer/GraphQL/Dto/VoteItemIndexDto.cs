namespace TomorrowDAOIndexer.GraphQL.Dto;

public class VoteItemIndexDto : BlockInfoDto
{
    // The voting activity id.(proposal id/customize)
    public string VotingItemId { get; set; }
    
    public string? Executer { get; set; }

    public string VoteSchemeId { get; set; }

    public string DAOId { get; set; }

    public string AcceptedCurrency { get; set; }

    public long ApprovedCount { get; set; }

    public long RejectionCount { get; set; }

    public long AbstentionCount { get; set; }

    public long VotesAmount { get; set; }
    public long VoterCount { get; set; }
    
    public DateTime RegisterTime { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
    
    public DateTime CreateTime { get; set; }
}