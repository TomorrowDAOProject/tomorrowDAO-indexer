
using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class ElectionHighCouncilConfigIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DaoId { get; set; }
    public long MaxHighCouncilMemberCount { get; set; }
    public long MaxHighCouncilCandidateCount { get; set; }
    public long ElectionPeriod { get; set; }
    public bool IsRequireHighCouncilForExecution { get; set; }
    [Keyword] public string GovernanceToken { get; set; }
    public long StakeThreshold { get; set; }
    public List<string> InitialHighCouncilMembers { get; set; } = new List<string>();
}