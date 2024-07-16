namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetDAOVoterRecordInput
{
    public string ChainId { get; set; }
    public string DaoId { get; set; }
    public string VoterAddress { get; set; }
}