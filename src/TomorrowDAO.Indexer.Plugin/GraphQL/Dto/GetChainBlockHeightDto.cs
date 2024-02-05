namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetChainBlockHeightInput
{
    public int SkipCount { get; set; }
    
    public string ChainId { get; set; }
    
    public long StartBlockHeight { get; set; }
    
    public long EndBlockHeight { get; set; }
}