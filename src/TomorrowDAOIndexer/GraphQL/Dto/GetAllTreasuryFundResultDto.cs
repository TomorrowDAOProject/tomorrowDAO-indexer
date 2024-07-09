namespace TomorrowDAOIndexer.GraphQL.Dto;

public class GetAllTreasuryFundResultDto
{
    public List<TreasuryFundDto> Data { get; set; }
    public long TotalCount { get; set; }
}