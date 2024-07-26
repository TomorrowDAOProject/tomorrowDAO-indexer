namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetProposalCountInput
{
    public string? ChainId { get; set; }
    public string? DaoId { get; set; }

    public string? StartTime { get; set; }

    public string? EndTime { get; set; }
}