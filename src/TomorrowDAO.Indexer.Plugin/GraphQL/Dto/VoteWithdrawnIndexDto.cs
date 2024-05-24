
namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class VoteWithdrawnIndexDto
{
    public string Id { get; set; }
    public string DaoId { get; set; }
    public string Voter { get; set; }
    public long WithdrawAmount { get; set; }
    public DateTime WithdrawTimestamp { get; set; }
    public List<string> VotingItemIdList { get; set; }
    public DateTime CreateTime { get; set; }
}