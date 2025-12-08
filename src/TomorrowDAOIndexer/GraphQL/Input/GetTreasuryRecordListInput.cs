namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetTreasuryRecordListInput : GetChainBlockHeightInput
{
    public string? DaoId { get; set; }
    public string? TreasuryAddress { get; set; }

    public string? FromAddress { get; set; }

    public List<string>? Symbols { get; set; }
}