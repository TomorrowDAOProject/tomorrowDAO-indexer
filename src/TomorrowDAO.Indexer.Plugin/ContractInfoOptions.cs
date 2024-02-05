namespace TomorrowDAO.Indexer.Plugin;

public class ContractInfoOptions
{
    public Dictionary<string, ContractInfo> ContractInfos { get; set; }
}
public class ContractInfo
{
    public string GovernanceContract  { get; set; }
    public string DAOContractAddress { get; set; }
    public string ElectionContractAddress { get; set; }
    public string VoteContractAddress { get; set; }
    public string TreasuryContractAddress { get; set; }
}