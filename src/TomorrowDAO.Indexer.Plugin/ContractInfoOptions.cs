namespace TomorrowDAO.Indexer.Plugin;

public class ContractInfoOptions
{
    public Dictionary<string, ContractInfo> ContractInfos { get; set; }
}

public class ContractInfo
{
    public string GovernanceContractAddress { get; set; }
    public string DAOContractAddress { get; set; }
    public string ElectionContractAddress { get; set; }
    public string VoteContractAddress { get; set; }
    public string TreasuryContractAddress { get; set; }

    public string TokenContractAddress { get; set; }

    public string ParliamentContractAddress { get; set; }
    public string AssociationContractAddress { get; set; }
    public string ReferendumContractAddress { get; set; }
}