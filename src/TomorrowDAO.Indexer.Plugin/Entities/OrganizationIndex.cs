using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class OrganizationIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [PropertyName("DAOId")] 
    [Keyword] public string DAOId { get; set; }
    [Keyword] public string Address { get; set; }
    public DateTime CreateTime { get; set; }
}