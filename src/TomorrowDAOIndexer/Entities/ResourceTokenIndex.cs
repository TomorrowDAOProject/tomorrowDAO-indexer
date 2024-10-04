using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class ResourceTokenIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string TransactionId { get; set; }
    [Keyword] public string Method { get; set; }
    [Keyword] public string Symbol { get; set; }
    public long ResourceAmount { get; set; }
    public long BaseAmount { get; set; } //elf
    public long FeeAmount { get; set; }
    [Keyword] public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    [Keyword] public string TransactionStatus { get; set; }
    public DateTime OperateTime { get; set; }
}