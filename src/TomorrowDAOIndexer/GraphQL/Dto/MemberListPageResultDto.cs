namespace TomorrowDAOIndexer.GraphQL.Dto;

public class MemberListPageResultDto
{
    public long TotalCount { get; set; }
    public List<MemberDto> Data { get; set; }
}