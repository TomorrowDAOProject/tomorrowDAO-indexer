using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class VoteSchemeIndexDto : BlockInfoDto
{
    public string Id { get; set; }
    public string VoteSchemeId { get; set; }
    public int VoteMechanism { get; set; }
    public bool IsLockToken { get; set; }
    public bool IsQuadratic { get; set; }
    public long TicketCost { get; set; }
    public DateTime CreateTime { get; set; }
    public bool WithoutLockToken { get; set; }
    public VoteStrategy VoteStrategy { get; set; }
    public int VoteCount { get; set; }
}