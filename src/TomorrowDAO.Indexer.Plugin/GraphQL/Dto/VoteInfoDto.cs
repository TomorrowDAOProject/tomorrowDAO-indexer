namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class VoteInfoDto
{
    // The voting activity id.(proposal id/customize)
    public string VotingItemId { get; set; }

    public string VoteSchemeId { get; set; }

    public string DAOId { get; set; }

    public string AcceptedCurrency { get; set; }

    public int ApproveCounts { get; set; }

    public int RejectCounts { get; set; }

    public int AbstainCounts { get; set; }

    public int VotesAmount { get; set; }
}