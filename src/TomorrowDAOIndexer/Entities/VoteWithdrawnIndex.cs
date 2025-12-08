using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class VoteWithdrawnIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    public long BlockHeight { get; set; }
    [Keyword] public string DaoId { get; set; }
    [Keyword] public string Voter { get; set; }
    public long WithdrawAmount { get; set; }
    public DateTime WithdrawTimestamp { get; set; }
    public List<string> VotingItemIdList { get; set; }
    public DateTime CreateTime { get; set; }
}