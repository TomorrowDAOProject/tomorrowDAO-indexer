using TomorrowDAO.Indexer.Plugin.Entities;

namespace TomorrowDAO.Indexer.Plugin.GraphQL.Dto;

public class TreasuryFundDto
{
    public string Id { get; set; }
    public string ChainId { get; set; }
    public long BlockHeight { get; set; }
    public string DAOId { get; set; }
    public string Symbol { get; set; }
    public long AvailableFunds { get; set; }
    public long LockedFunds { get; set; }
    public bool IsRemoved { get; set; }
}