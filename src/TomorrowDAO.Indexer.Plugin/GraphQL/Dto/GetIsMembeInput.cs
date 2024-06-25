namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetIsMemberInput
{
    public string ChainId { get; set; }
    public string DAOId { get; set; }
    public string Address { get; set; }
}