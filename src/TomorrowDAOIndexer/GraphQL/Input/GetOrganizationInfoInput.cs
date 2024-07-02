namespace TomorrowDAOIndexer.GraphQL.Input;

public class GetOrganizationInfoInput
{
    public string ChainId { get; set; }
    
    public List<string> OrganizationAddressList { get; set; }
    
    public string GovernanceSchemeId { get; set; }
}