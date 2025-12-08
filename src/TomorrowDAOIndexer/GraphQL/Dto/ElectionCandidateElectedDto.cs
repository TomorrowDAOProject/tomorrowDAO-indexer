namespace TomorrowDAOIndexer.GraphQL.Dto;

public class ElectionCandidateElectedDto
{
    public long TotalCount { get; set; }
    public List<ElectionCandidateElected> Items { get; set; }
}

public class ElectionCandidateElected : BlockInfoDto
{
    public string Id { get; set; }
    public string DaoId { get; set; }
    public long PreTermNumber { get; set; }
    public long NewNumber { get; set; }
    public DateTime CandidateElectedTime { get; set; }
}

public class GetElectionCandidateElectedInput
{
    public string ChainId { get; set; }
    public string DaoId { get; set; }
    public int SkipCount { get; set; } = 0;
    public int MaxResultCount { get; set; } = 10;
    public long StartBlockHeight { get; set; }
    public long EndBlockHeight { get; set; }
}