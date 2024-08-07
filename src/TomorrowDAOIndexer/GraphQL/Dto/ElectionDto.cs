using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class ElectionDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    public string DAOId { get; set; }
    public long TermNumber { get; set; }
    public HighCouncilType HighCouncilType { get; set; }
    public string Address { get; set; }
    public long VotesAmount { get; set; }
    public long StakeAmount { get; set; }
}