using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class NetworkDaoProposalIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public string ProposalId { get; set; }

    [Keyword] public string OrganizationAddress { get; set; }

    [Keyword] public string Title { get; set; }

    public string Description { get; set; }

    public NetworkDaoProposalType ProposalType { get; set; }
}