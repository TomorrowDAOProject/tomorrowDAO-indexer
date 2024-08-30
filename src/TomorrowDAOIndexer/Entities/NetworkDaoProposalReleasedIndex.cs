using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoProposalReleasedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string ProposalId { get; set; }
    [Keyword] public string OrganizationAddress { get; set; }
    [Keyword] public string Title { get; set; }
    public string Description { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword]
    public NetworkDaoOrgType OrgType { get; set; }
    public long BlockHeight { get; set; }
}