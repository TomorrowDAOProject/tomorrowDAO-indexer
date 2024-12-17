using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Entities.Base;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoOrgMemberChangedIndex : AeFinderEntity, IAeFinderEntity, ITransactionEntity
{
    [Keyword] public string OrganizationAddress { get; set; }
    [Keyword] public string AddedAddress { get; set; }
    [Keyword] public string RemovedAddress { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword]
    public NetworkDaoOrgType OrgType { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword]
    public OrgMemberChangeTypeEnum ChangeType { get; set; }

    public long BlockHeight { get; set; }
    public TransactionInfo TransactionInfo { get; set; }
}