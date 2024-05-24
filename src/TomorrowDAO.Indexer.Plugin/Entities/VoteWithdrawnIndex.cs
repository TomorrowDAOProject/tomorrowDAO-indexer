using AElf.Indexing.Elasticsearch;
using AElfIndexer.Client;
using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class VoteWithdrawnIndex : AElfIndexerClientEntity<string>, IIndexBuild
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string DaoId { get; set; }
    [Keyword] public string Voter { get; set; }
    public long WithdrawAmount { get; set; }
    public DateTime WithdrawTimestamp { get; set; }
    public List<string> VotingItemIdList { get; set; }
    public DateTime CreateTime { get; set; }
}