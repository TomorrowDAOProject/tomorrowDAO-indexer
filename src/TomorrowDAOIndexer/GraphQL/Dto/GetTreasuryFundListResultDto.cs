namespace TomorrowDAOIndexer.GraphQL.Dto;

public class GetTreasuryFundListResultDto
{
    public List<TreasuryFundDto> Data { get; set; }
    public long TotalCount { get; set; }
}