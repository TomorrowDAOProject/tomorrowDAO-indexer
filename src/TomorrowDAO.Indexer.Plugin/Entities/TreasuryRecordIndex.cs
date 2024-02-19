using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class TreasuryRecordIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [PropertyName("DAOId")]
    [Keyword] public string DAOId { get; set; }
    public long Amount { get; set; }
    [Keyword] public string Symbol { get; set; }
    [Keyword] public string Executor { get; set; }
    [Keyword] public string From { get; set; }
    [Keyword] public string To { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public TreasuryRecordType TreasuryRecordType { get; set; }
    public DateTime CreateTime { get; set; }
    
}