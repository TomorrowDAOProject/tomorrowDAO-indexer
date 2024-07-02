using AeFinder.Sdk.Entities;
using Nest;

namespace TomorrowDAOIndexer.Entities;

public class OrganizationIndex : AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [PropertyName("DAOId")] 
    [Keyword] public string DAOId { get; set; }
    [Keyword] public string Address { get; set; }
    public DateTime CreateTime { get; set; }
}