namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetAllVoteRecordInput
{
    public string ChainId { get; set; }

    public string DAOId { get; set; }
    
    public string Voter { get; set; }
}