using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class TreasuryRecordDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    public string DAOId { get; set; }
    public long Amount { get; set; }
    public string Symbol { get; set; }
    public string Executor { get; set; }
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public TreasuryRecordType TreasuryRecordType { get; set; }
    public DateTime CreateTime { get; set; }
}