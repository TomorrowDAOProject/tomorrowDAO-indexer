using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class GetNetworkDaoDataInput
{
    public string? ChainId { get; set; }
    public List<string>? OrgAddresses { get; set; }
    public NetworkDaoOrgType OrgType { get; set; } = NetworkDaoOrgType.All;
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
}

public class NetworkDaoOrgChangedIndexDto : BlockInfoDto
{
    public string OrganizationAddress { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoDataChangedIndexInput : GetNetworkDaoDataInput
{
}

public class NetworkDaoOrgCreatedIndexDto : BlockInfoDto
{
    public string OrganizationAddress { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoDataCreatedIndexInput : GetNetworkDaoDataInput
{
}

public class NetworkDaoOrgThresholdChangedIndexDto : BlockInfoDto
{
    public string OrganizationAddress { get; set; }
    public long MinimalApprovalThreshold { get; set; }
    public long MaximalRejectionThreshold { get; set; }
    public long MaximalAbstentionThreshold { get; set; }
    public long MinimalVoteThreshold { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoDataThresholdChangedIndexInput : GetNetworkDaoDataInput
{
}

public class NetworkDaoOrgWhiteListChangedIndexDto : BlockInfoDto
{
    public string OrganizationAddress { get; set; }
    public List<string> ProposerWhiteList { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoDataWhiteListChangedIndexInput : GetNetworkDaoDataInput
{
}

public class NetworkDaoOrgMemberChangedIndexDto : BlockInfoDto
{
    public string OrganizationAddress { get; set; }
    public string AddedAddress { get; set; }
    public string RemovedAddress { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public OrgMemberChangeTypeEnum ChangeType { get; set; }
    public TransactionInfoDto TransactionInfo { get; set; }
}

public class GetNetworkDaoDataMemberChangedIndexInput : GetNetworkDaoDataInput
{
    public OrgMemberChangeTypeEnum ChangeType { get; set; } = OrgMemberChangeTypeEnum.All;
}