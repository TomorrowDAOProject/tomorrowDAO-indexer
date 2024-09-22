namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetBalanceInput
{
    public string ChainId { get; set; }
    public string Address { get; set; }
}