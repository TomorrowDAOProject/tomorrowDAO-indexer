namespace TomorrowDAOIndexer.GraphQL.Dto;

public class VoteItemIndexDto : BlockInfoDto
{
    // The voting activity id.(proposal id/customize)
    public string VotingItemId { get; set; }
    
    public string Executer { get; set; }

    public string VoteSchemeId { get; set; }

    public string DAOId { get; set; }

    public string AcceptedCurrency { get; set; }

    public int ApprovedCount { get; set; }

    public int RejectionCount { get; set; }

    public int AbstentionCount { get; set; }

    public int VotesAmount { get; set; }
    public int VoterCount { get; set; }
    
    public DateTime RegisterTime { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
    
    public DateTime CreateTime { get; set; }
}