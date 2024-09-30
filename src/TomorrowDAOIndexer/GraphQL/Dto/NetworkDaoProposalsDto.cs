using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

[Obsolete]
public class NetworkDaoProposalsDto
{
    public long TotalCount { get; set; }
    public List<NetworkDaoProposal> Items { get; set; }
}

[Obsolete]
public class NetworkDaoProposal : BlockInfoDto
{
    public string Id { get; set; }
    public string ProposalId { get; set; }
    public string OrganizationAddress { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ProposalType { get; set; }
}

[Obsolete]
public class GetNetworkDaoProposalsInput
{
    public string? ChainId { get; set; }
    public List<string?>? ProposalIds { get; set; }
    public NetworkDaoProposalType ProposalType { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
}

public class NetworkDaoProposalPageResultDto : NetworkDaoDataPageResultDto<NetworkDaoProposalIndexDto>
{
}

public class NetworkDaoProposalIndexDto : BlockInfoDto
{
    public string Id { get; set; }
    public string ProposalId { get; set; }
    public string OrganizationAddress { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public bool IsReleased { get; set; }
    public DateTime SaveTime { get; set; }
    public string Symbol { get; set; }
    public long TotalAmount { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoProposalIndexInput : GetNetworkDaoDataInput
{
    public List<string> ProposalIds { get; set; }
    public string Title { get; set; }
}

public class NetworkDaoProposalReleasedPageResultDto : NetworkDaoDataPageResultDto<NetworkDaoProposalReleasedIndexDto>
{
}

public class NetworkDaoProposalReleasedIndexDto  : BlockInfoDto
{
    public string Id { get; set; }
    public string ProposalId { get; set; }
    public string OrganizationAddress { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoProposalReleasedIndexInput : GetNetworkDaoDataInput
{
    public List<string> ProposalIds { get; set; }
    public string Title { get; set; }
}

public class NetworkDaoProposalVoteRecordPageResultDto : NetworkDaoDataPageResultDto<NetworkDaoProposalVoteRecordIndexDto>
{
}

public class NetworkDaoProposalVoteRecordIndexDto : BlockInfoDto
{
    public string Id { get; set; }
    public string ProposalId { get; set; }
    public string Address { get; set; }
    //Approve, Reject or Abstain
    public ReceiptTypeEnum ReceiptType { get; set; }
    public DateTime Time { get; set; }
    public string OrganizationAddress { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public string Symbol { get; set; }
    public long Amount { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoProposalVoteRecordIndexInput : GetNetworkDaoDataInput
{
    public List<string> ProposalIds { get; set; }

    public ReceiptTypeEnum ReceiptType { get; set; } = ReceiptTypeEnum.All;
}