using Nest;

namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetDAOAmountRecordDto
{
    public string GovernanceToken { get; set; }
    public long Amount { get; set; }
}