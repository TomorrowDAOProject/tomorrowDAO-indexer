using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class VoteSchemeInfoDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    public string VoteSchemeId { get; set; }
    public VoteMechanism VoteMechanism { get; set; }
    public bool IsLockToken { get; set; }
    public bool IsQuadratic { get; set; }
    public long TicketCost { get; set; }
    public DateTime CreateTime { get; set; }
    public bool WithoutLockToken { get; set; }
    public VoteStrategy VoteStrategy { get; set; }
}