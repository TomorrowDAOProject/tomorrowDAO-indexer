namespace TomorrowDAOIndexer.GraphQL.Dto;

public class DAUCountDto
{
    public long Dau { get; set; }
    public long DauDaoCreate { get; set; }
    public long DauVote { get; set; }
    public long DauProposalCreate { get; set; }
    public long DauProposalExecute { get; set; }
    public long DauTreasury { get; set; }
}