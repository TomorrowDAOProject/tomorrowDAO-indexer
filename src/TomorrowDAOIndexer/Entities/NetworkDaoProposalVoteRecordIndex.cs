using AeFinder.Sdk.Entities;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using TomorrowDAOIndexer.Entities.Base;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoProposalVoteRecordIndex : AeFinderEntity, IAeFinderEntity, ITransactionEntity
{
    [Keyword] public string ProposalId { get; set; }

    [Keyword] public string Address { get; set; }
    
    //Approve, Reject or Abstain
    [JsonConverter(typeof(StringEnumConverter))]
    [Keyword] public ReceiptTypeEnum ReceiptType { get; set; }

    //The timestamp of this method call
    public DateTime Time { get; set; }
    [Keyword] public string OrganizationAddress { get; set; }
    [Keyword] public NetworkDaoOrgType OrgType { get; set; }
    [Keyword] public string Symbol { get; set; }
    public long Amount { get; set; }
    public long BlockHeight { get; set; }
    public TransactionInfo TransactionInfo { get; set; }
}