namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class DaoVoterRecordIndexDto
{
    public string Id { get; set; }
    public string DaoId { get; set; }
    public string VoterAddress { get; set; }
    public int Count { get; set; }
    public string ChainId { get; set; }
}