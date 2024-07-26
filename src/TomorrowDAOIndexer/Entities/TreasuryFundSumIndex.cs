using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class TreasuryFundSumIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    public long BlockHeight { get; set; }
    [Keyword] public string Symbol { get; set; }
    public long AvailableFunds { get; set; }
}