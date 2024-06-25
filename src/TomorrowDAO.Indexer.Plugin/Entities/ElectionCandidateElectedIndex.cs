using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class ElectionCandidateElectedIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DaoId { get; set; }

    public long PreTermNumber { get; set; }
    public long NewNumber { get; set; }
    public DateTime CandidateElectedTime { get; set; }
}