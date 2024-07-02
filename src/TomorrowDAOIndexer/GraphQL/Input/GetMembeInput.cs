namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetMemberInput
{
    public string ChainId { get; set; }
    public string DAOId { get; set; }
    public string Address { get; set; }
}