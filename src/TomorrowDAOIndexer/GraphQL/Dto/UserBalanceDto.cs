namespace TomorrowDAOIndexer.GraphQL.Dto;

public class UserBalanceDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    
    public string Address { get; set; }
    
    public long Amount { get; set; }

    public string Symbol { get; set; }

    public DateTime ChangeTime { get; set; }
    public long BlockHeight { get; set; }
}