namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetVoteInfoInput
{
    public string ChainId { get; set; }
    
    public string VotingItemId { get; set; }
    
    public List<string> VotingItemIds { get; set; }
}