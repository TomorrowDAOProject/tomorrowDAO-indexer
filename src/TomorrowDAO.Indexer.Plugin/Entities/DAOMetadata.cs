using Nest;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class DAOMetadata
{
    [Keyword] public string Name { get; set; }
    
    [Keyword] public string LogoUrl { get; set; }
    
    [Keyword] public string Description { get; set; }
    
    [Keyword] public string SocialMedia { get; set; }
}