using TomorrowDAO.Indexer.Plugin.Enums;

namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GovernanceMode
{
    public string Id { get; set; }
    public GovernanceMechanism GovernanceMechanism { get; set; }
}


