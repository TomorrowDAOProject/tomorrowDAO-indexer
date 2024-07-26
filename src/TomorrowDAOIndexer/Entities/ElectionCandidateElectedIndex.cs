using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class ElectionCandidateElectedIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public string DaoId { get; set; }
    public long BlockHeight { get; set; }
    public long PreTermNumber { get; set; }
    public long NewNumber { get; set; }
    public DateTime CandidateElectedTime { get; set; }
}