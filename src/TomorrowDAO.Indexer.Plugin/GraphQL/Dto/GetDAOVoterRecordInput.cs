namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetDAOVoterRecordInput
{
    public string ChainId { get; set; }
    public List<string> DaoIds { get; set; }
    public List<string> VoterAddressList { get; set; }
}