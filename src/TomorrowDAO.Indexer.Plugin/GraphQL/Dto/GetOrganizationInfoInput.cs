namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetOrganizationInfoInput
{
    public string ChainId { get; set; }
    
    public List<string> OrganizationAddressList { get; set; }
    
    public string GovernanceSchemeId { get; set; }
}