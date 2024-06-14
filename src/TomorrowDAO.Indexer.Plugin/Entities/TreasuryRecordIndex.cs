using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class TreasuryRecordIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DaoId { get; set; }
    [Keyword] public string TreasuryAddress { get; set; }
    public long Amount { get; set; }
    [Keyword] public string Symbol { get; set; }
    [Keyword] public string FromAddress { get; set; }
    [Keyword] public string ToAddress { get; set; }
    [Keyword] public string Executor { get; set; }
    public string Memo { get; set; }
    public TreasuryRecordType TreasuryRecordType { get; set; }
    public DateTime CreateTime { get; set; }
    public string ProposalId { get; set; }
}