using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class TreasuryCreateIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string DaoId { get; set; }
    public long BlockHeight { get; set; }
    [Keyword] public string TreasuryAddress { get; set; }
}