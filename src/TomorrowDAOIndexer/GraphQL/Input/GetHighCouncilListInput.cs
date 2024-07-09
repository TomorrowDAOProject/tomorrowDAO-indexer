using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetHighCouncilListInput
{
    public string ChainId { get; set; }
    public string DAOId { get; set; }
    public long TermNumber { get; set; }
    public HighCouncilType HighCouncilType { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; } 
}