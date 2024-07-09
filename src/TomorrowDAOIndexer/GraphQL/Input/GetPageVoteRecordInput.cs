using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetPageVoteRecordInput
{
    public string ChainId { get; set; }
    
    public string DaoId { get; set; }
    
    public string Voter { get; set; }

    public string VotingItemId { get; set; }
    
    public int SkipCount { get; set; }
    
    public int MaxResultCount { get; set; }
    
    public VoteOption VoteOption { get; set; }
}