namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetDaoCountInput
{
    public string? ChainId { get; set; }

    public string? StartTime { get; set; }

    public string? EndTime { get; set; }
}