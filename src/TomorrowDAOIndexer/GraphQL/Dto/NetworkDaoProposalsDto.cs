using Microsoft.Extensions.Logging;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.GraphQL.Dto;

public class NetworkDaoProposalsDto
{
    public long TotalCount { get; set; }
    public List<NetworkDaoProposal> Items { get; set; }
}

public class NetworkDaoProposal : BlockInfoDto
{
    public string Id { get; set; }
    public string ProposalId { get; set; }
    public string OrganizationAddress { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int ProposalType { get; set; }
}

public class GetNetworkDaoProposalsInput
{
    public string? ChainId { get; set; }
    public List<string?>? ProposalIds { get; set; }
    public NetworkDaoOrgType OrgType { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
}