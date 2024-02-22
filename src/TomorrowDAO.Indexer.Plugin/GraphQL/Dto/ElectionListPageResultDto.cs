namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class ElectionListPageResultDto
{
    public long TotalCount { get; set; }
    public List<ElectionDto> DataList { get; set; }
}