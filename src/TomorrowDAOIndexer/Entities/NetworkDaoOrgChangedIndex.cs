using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoOrgChangedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword]
    public string OrganizationAddress { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword]
    public NetworkDaoOrgType OrgType { get; set; }
    public long BlockHeight { get; set; }
}