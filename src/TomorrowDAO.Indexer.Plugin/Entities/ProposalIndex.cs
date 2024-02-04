using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAOServer.Enums;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class ProposalIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }

    [Keyword] public string DaoId { get; set; }

    [Keyword] public string ProposalId { get; set; }

    [Keyword] public string ProposalTitle { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ProposalType ProposalType { get; set; }

    //get from GovernanceSchemeId
    [JsonConverter(typeof(StringEnumConverter))]
    public GovernanceType GovernanceType { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ProposalStatus ProposalStatus { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime ExpiredTime { get; set; }

    [Keyword] public string Proposer { get; set; }

    [Keyword] public string OrganizationAddress { get; set; }

    [Keyword] public string ReleaseAddress { get; set; }

    [Keyword] public string ProposalDescriptionUrl { get; set; }

    public CallTransaction Transaction { get; set; }

    //sub_scheme_id
    [Keyword] public string GovernanceSchemeId { get; set; }

    [Keyword] public string VoteSchemeId { get; set; }

    public bool ExecuteByHighCouncil { get; set; }

    public DateTime DeployTime { get; set; }
}