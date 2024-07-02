namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetDAOVoterRecordInput
{
    public string ChainId { get; set; }
    public List<string> DaoIds { get; set; }
    public List<string> VoterAddressList { get; set; }
}