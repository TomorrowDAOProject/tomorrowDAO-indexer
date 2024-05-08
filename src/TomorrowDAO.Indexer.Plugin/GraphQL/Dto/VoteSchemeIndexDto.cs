namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class VoteSchemeIndexDto
{
    public string Id { get; set; }
    public string VoteSchemeId { get; set; }
    public int VoteMechanism { get; set; }
    public bool IsLockToken { get; set; }
    public bool IsQuadratic { get; set; }
    public long TicketCost { get; set; }
    public DateTime CreateTime { get; set; }
}