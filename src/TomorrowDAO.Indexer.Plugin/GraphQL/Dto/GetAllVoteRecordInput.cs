namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetAllVoteRecordInput
{
    public string ChainId { get; set; }

    public string DAOId { get; set; }
    
    public string Voter { get; set; }
}