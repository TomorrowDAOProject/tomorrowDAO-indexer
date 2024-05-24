namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetHighCouncilListInput
{
    public string ChainId { get; set; }
    public string DAOId { get; set; }
    public long TermNumber { get; set; }
    public string HighCouncilType { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; } 
    public string Sorting { get; set; } 
}