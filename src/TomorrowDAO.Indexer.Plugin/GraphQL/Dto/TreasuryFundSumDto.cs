namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class TreasuryFundSumDto
{
    public string ChainId { get; set; }
    public string Symbol { get; set; }
    public long AvailableFunds { get; set; }
}