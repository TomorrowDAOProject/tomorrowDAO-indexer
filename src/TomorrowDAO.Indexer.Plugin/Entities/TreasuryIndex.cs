using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using TomorrowDAO.Contracts.DAO;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class TreasuryIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string TreasuryContractAddress { get; set; }
    [Keyword] public string SymbolList { get; set; }
}