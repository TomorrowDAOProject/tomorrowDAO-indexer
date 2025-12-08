namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetTreasuryFundListInput : GetChainBlockHeightInput
{
    public string? DaoId { get; set; }
    public string? TreasuryAddress { get; set; }

    public List<string>? Symbols { get; set; }
}