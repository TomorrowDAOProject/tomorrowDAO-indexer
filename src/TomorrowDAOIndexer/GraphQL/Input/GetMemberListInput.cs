namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetMemberListInput
{
    public string ChainId { get; set; }
    public string DAOId { get; set; }
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; } 
}