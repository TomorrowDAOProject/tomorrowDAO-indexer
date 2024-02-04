using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class ElectionIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DaoId { get; set; }
    public long TermNumber { get; set; }
    public HighCouncilType HighCouncilType { get; set; }
    public string Address { get; set; }
}