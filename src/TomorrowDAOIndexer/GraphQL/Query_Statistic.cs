using System.Globalization;
using System.Linq.Dynamic.Core;
using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getDAU")]
    public static async Task<DAUCountDto> GetDAUAsync(
        [FromServices] IReadOnlyRepository<DAOIndex> daoRepository,
        [FromServices] IReadOnlyRepository<VoteRecordIndex> voteRecordRepository,
        [FromServices] IReadOnlyRepository<ProposalIndex> proposalRepository,
        [FromServices] IReadOnlyRepository<TreasuryRecordIndex> treasuryRecordRepository,
        [FromServices] IObjectMapper objectMapper, GetDAUInput input)
    {
        var startTime = string.IsNullOrEmpty(input.StartTime) ? new DateTime(1970, 1, 1)  :
            DateTime.ParseExact(input.StartTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
        var endTime = DateTime.ParseExact(input.EndTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
        
        // dao
        var daoQueryAble = await daoRepository.GetQueryableAsync();
        daoQueryAble = daoQueryAble.Where(a => a.CreateTime >= startTime).Where(a => a.CreateTime <= endTime);
        var daoList = QueryHelper.GetAllIndex(daoQueryAble).Select(x => x.Creator).Distinct().ToList();

        // vote
        var voteQueryAble = await voteRecordRepository.GetQueryableAsync();
        voteQueryAble = voteQueryAble.Where(a => a.VoteTimestamp >= startTime).Where(a => a.VoteTimestamp <= endTime);
        var voteRecordList = QueryHelper.GetAllIndex(voteQueryAble).Select(x => x.Voter).Distinct().ToList();
        
        // proposal create
        var proposalQueryAble = await proposalRepository.GetQueryableAsync();
        proposalQueryAble = proposalQueryAble.Where(a => a.DeployTime >= startTime).Where(a => a.DeployTime <= endTime);
        var proposalList = QueryHelper.GetAllIndex(proposalQueryAble).Select(x => x.Proposer).Distinct().ToList();
        
        // proposal execute
        var proposalExecuteQueryAble = await proposalRepository.GetQueryableAsync();
        proposalExecuteQueryAble = proposalExecuteQueryAble.Where(a => a.ExecuteTime >= startTime).Where(a => a.ExecuteTime <= endTime);
        var proposalExecuteList = QueryHelper.GetAllIndex(proposalExecuteQueryAble).ToList().Select(x => x.Proposer).Distinct().ToList();
        
        // treasury
        var treasuryQueryAble = await treasuryRecordRepository.GetQueryableAsync();
        treasuryQueryAble = treasuryQueryAble.Where(a => a.CreateTime >= startTime).Where(a => a.CreateTime <= endTime);
        var treasuryList = QueryHelper.GetAllIndex(treasuryQueryAble)
            .Where(x => x.TreasuryRecordType == TreasuryRecordType.Deposit).Select(x => x.FromAddress).Distinct().ToList();

        var count = daoList
            .Concat(voteRecordList)
            .Concat(proposalList)
            .Concat(proposalExecuteList)
            .Concat(treasuryList)
            .Distinct()
            .Count();
        return new DAUCountDto
        {
            Dau = count,
            DauDaoCreate = daoList.Count,
            DauVote = voteRecordList.Count,
            DauProposalCreate = proposalList.Count,
            DauProposalExecute = proposalExecuteList.Count,
            DauTreasury = treasuryList.Count
        };
    }
}