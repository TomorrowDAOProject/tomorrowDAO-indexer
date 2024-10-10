using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Entities.Base;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoProposalReleasedIndex : AeFinderEntity, IAeFinderEntity, ITransactionEntity
{
    [Keyword] public string ProposalId { get; set; }
    [Keyword] public string OrganizationAddress { get; set; }
    [Keyword] public string Title { get; set; }
    public string Description { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword]
    public NetworkDaoOrgType OrgType { get; set; }
    public long BlockHeight { get; set; }
    public TransactionInfo TransactionInfo { get; set; }
}