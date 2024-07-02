using Nest;

namespace TomorrowDAOIndexer.Entities;

public class Metadata
{
    [Keyword] public string Name { get; set; }
    
    [Keyword] public string LogoUrl { get; set; }
    
    [Keyword] public string Description { get; set; }
    
    [Keyword] public string SocialMedia { get; set; }
}