namespace TomorrowDAO.Indexer.Plugin.Entities;

public class CallTransaction
{
    // The address of the target contract.
    public string ToAddress { get; set; }

    // The method that this proposal will call when being released.
    public string ContractMethodName { get; set; }

    public object[] Params { get; set; }
}