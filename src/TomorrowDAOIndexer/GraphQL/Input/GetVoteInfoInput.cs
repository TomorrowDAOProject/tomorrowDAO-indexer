namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetVoteInfoInput
{
    public string ChainId { get; set; }
    
    public List<string> VotingItemIds { get; set; }
    
    public List<string> DaoIds { get; set; }
}