using AeFinder.Sdk.Entities;
using Nest;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Entities;

public class NetworkDaoProposalIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string ProposalId { get; set; }
    public long BlockHeight { get; set; }

    [Keyword] public string OrganizationAddress { get; set; }

    [Keyword] public string Title { get; set; }

    public string Description { get; set; }

    public NetworkDaoProposalType ProposalType { get; set; }
}