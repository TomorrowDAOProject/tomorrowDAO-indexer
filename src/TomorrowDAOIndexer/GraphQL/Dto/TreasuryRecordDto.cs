namespace TomorrowDAOIndexer.GraphQL.Dto;

public class GetTreasuryRecordListResultDto
{
    public List<TreasuryRecordDto> Item2 { get; set; }
    public long Item1 { get; set; }
}

public class TreasuryRecordDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    public string DaoId { get; set; }
    public string TreasuryAddress { get; set; }
    public long Amount { get; set; }
    public string Symbol { get; set; }
    public string Executor { get; set; }
    public string? FromAddress { get; set; }
    public string? ToAddress { get; set; }
    public string? Memo { get; set; }
    public int TreasuryRecordType { get; set; }
    public DateTime CreateTime { get; set; }
    public string? ProposalId { get; set; }
}