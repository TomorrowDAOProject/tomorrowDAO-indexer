using Nest;

namespace TomorrowDAOIndexer.Entities;

public class ExecuteTransaction
{
    // The address of the target contract.
    [Keyword] public string ToAddress { get; set; }

    // The method that this proposal will call when being released.
    [Keyword] public string ContractMethodName { get; set; }

    //key is paramName, value is param value
    public string Params { get; set; }
}