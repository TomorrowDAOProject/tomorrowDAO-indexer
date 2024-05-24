
namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class VoteWithdrawnIndexInput
{
    public string ChainId { get; set; }
    public string DaoId { get; set; }
    public string Voter { get; set; }
}