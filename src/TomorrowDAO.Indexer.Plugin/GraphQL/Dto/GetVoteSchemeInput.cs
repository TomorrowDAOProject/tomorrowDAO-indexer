namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetVoteSchemeInput
{
    public string ChainId { get; set; }
    public List<int> Types { get; set; }
}