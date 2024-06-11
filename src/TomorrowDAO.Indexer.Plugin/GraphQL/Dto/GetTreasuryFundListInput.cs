namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class GetTreasuryFundListInput : GetChainBlockHeightInput
{
    public string DaoId { get; set; }
    public string TreasuryAddress { get; set; }
}