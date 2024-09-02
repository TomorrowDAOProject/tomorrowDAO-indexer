using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoOrgThresholdChangedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword]
    public string OrganizationAddress { get; set; }
    public long MinimalApprovalThreshold { get; set; }
    public long MaximalRejectionThreshold { get; set; }
    public long MaximalAbstentionThreshold { get; set; }
    public long  MinimalVoteThreshold { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword]
    public NetworkDaoOrgType OrgType { get; set; }
    public long BlockHeight { get; set; }
}

