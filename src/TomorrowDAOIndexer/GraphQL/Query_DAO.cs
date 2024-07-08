using System.Globalization;
using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;
using GetChainBlockHeightInput = TomorrowDAOIndexer.GraphQL.Input.GetChainBlockHeightInput;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getDAOList")]
    public static async Task<List<DAOInfoDto>> GetDAOListAsync(
        [FromServices] IReadOnlyRepository<DAOIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetChainBlockHeightInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        var accounts= queryable.ToList();
        return objectMapper.Map<List<DAOIndex>, List<DAOInfoDto>>(accounts);
    }
    
    [Name("getDaoCount")]
    public static async Task<long> GetDaoCountAsync([FromServices] IReadOnlyRepository<DAOIndex> repository, GetDaoCountInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        
        if (!input.StartTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.StartTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
            queryable = queryable.Where(a => a.CreateTime >= dateTime);
        }

        if (!input.EndTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.EndTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
            queryable = queryable.Where(a => a.CreateTime <= dateTime);
        }
        
        return queryable.Count();
    }
    
    [Name("getDAOVoterRecord")]
    public static async Task<List<DaoVoterRecordIndexDto>> GetDAOVoterRecordAsync(
        [FromServices] IReadOnlyRepository<DaoVoterRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetDAOVoterRecordInput input)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
    
        if (!input.DaoIds.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.DaoIds.Select(daoId => (Expression<Func<DaoVoterRecordIndex, bool>>)(o => o.DaoId == daoId))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
    
        if (input.VoterAddressList.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                input.VoterAddressList.Select(address => (Expression<Func<DaoVoterRecordIndex, bool>>)(o => o.VoterAddress == address))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        return objectMapper.Map<List<DaoVoterRecordIndex>, List<DaoVoterRecordIndexDto>>(queryable.ToList());
    }
    
    [Name("getDAOAmountRecord")]
    public static async Task<List<GetDAOAmountRecordDto>> GetDAOAmountRecordAsync(
        [FromServices] IReadOnlyRepository<DAOIndex> repository,
        [FromServices] IReadOnlyRepository<TreasuryFundIndex> treasuryFundrepository,
        [FromServices] IObjectMapper objectMapper,
        GetDAOAmountRecordInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        var result= queryable.ToList();
        var treasuryFunds = await GetTreasuryFundByFundListAsync(treasuryFundrepository, 
        new GetTreasuryFundInput { ChainId = input.ChainId });
        var voteFunds = objectMapper.Map<List<DAOIndex>, List<GetDAOAmountRecordDto>>(result);
        treasuryFunds.AddRange(voteFunds);
        return treasuryFunds.GroupBy(fund => fund.GovernanceToken)
            .Select(group => new GetDAOAmountRecordDto
            {
                GovernanceToken = group.Key,
                Amount = group.Sum(fund => fund.Amount)
            }).ToList();
    }

    [Name("getMyParticipated")]
    public static async Task<PageResultDto<DAOInfoDto>> GetMyParticipatedAsync(
        [FromServices] IReadOnlyRepository<DAOIndex> DAORepository,
        [FromServices] IReadOnlyRepository<LatestParticipatedIndex> participatedRepository,
        [FromServices] IObjectMapper objectMapper, GetParticipatedInput input)
    {
        var queryable = await participatedRepository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Where(a => a.Address == input.Address);
        var participatedCount = queryable.Count();

        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderByDescending(a => a.LatestParticipatedTime);
        var participatedResult = queryable.ToList();
        if (participatedResult.Count == 0)
        {
            return new PageResultDto<DAOInfoDto>(participatedCount, new List<DAOInfoDto>());
        }

        var daoIds = participatedResult.Select(x => x.DAOId).ToList();
        var daoQueryable = await DAORepository.GetQueryableAsync();
        daoQueryable = daoQueryable.Where(
            daoIds.Select(daoId => (Expression<Func<DAOIndex, bool>>)(o => o.Id == daoId))
            .Aggregate((prev, next) => prev.Or(next)));
        var daoResult = daoQueryable.ToList();
        
        return new PageResultDto<DAOInfoDto>
        {
            TotalCount = participatedCount,
            Data = objectMapper.Map<List<DAOIndex>, List<DAOInfoDto>>(daoResult)
        };
    }
}