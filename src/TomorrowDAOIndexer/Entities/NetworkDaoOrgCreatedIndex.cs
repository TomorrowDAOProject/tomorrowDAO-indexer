using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoOrgCreatedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword]
    public string OrganizationAddress { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword]
    public NetworkDaoOrgType OrgType { get; set; }
    public long BlockHeight { get; set; }
    
    public Transaction Transaction { get; set; }
}

public class Transaction
{
    public string TransactionId { get; set; }

    public string From { get; set; }

    public string To { get; set; }

    public string MethodName { get; set; }
}