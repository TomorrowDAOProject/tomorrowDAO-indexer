namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetDaoCountInput
{
    public string ChainId { get; set; }

    public string StartTime { get; set; }

    public string EndTime { get; set; }
}