namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class VoteInfoDto
{
    // The voting activity id.(proposal id/customize)
    public string VotingItemId { get; set; }

    public string VoteSchemeId { get; set; }

    public string DAOId { get; set; }

    public string AcceptedCurrency { get; set; }

    public int ApproveCount { get; set; }

    public int RejectCount { get; set; }

    public int AbstainCount { get; set; }

    public int VotesAmount { get; set; }
    
    public int VoterCount { get; set; }
}