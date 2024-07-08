using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getTreasuryFundByFundList")]
    public static async Task<List<GetDAOAmountRecordDto>> GetTreasuryFundByFundListAsync(
        [FromServices] IReadOnlyRepository<TreasuryFundIndex> repository, GetTreasuryFundInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        return queryable.ToList()
            .GroupBy(fund => fund.Symbol)
            .Select(group => new GetDAOAmountRecordDto
            {
                GovernanceToken = group.Key,
                Amount = group.Sum(fund => fund.AvailableFunds)
            }).ToList();
    }
    
    [Name("getTreasuryFund")]
    public static async Task<List<GetDAOAmountRecordDto>> GetTreasuryFundAsync(
        [FromServices] IReadOnlyRepository<TreasuryFundSumIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetTreasuryFundInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        return objectMapper.Map<List<TreasuryFundSumIndex>, List<GetDAOAmountRecordDto>>(queryable.ToList());
    }
    
    // todo server change
    [Name("getAllTreasuryFundList")]
    public static async Task<Tuple<long, List<TreasuryFundDto>>> GetAllTreasuryFundListAsync(
        [FromServices] IReadOnlyRepository<TreasuryFundIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetAllTreasuryFundListInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Where(a => a.DaoId == input.DaoId);
        
        var allFunds = objectMapper.Map<List<TreasuryFundIndex>, List<TreasuryFundDto>>(queryable.ToList());
        return new Tuple<long, List<TreasuryFundDto>>(allFunds.Count, allFunds);
    }
    
    // todo server change
    [Name("getTreasuryFundList")]
    public static async Task<Tuple<long, List<TreasuryFundDto>>> GetTreasuryFundListAsync(
        [FromServices] IReadOnlyRepository<TreasuryFundIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetTreasuryFundListInput input)
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
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DaoId == input.DaoId);
        }
        if (!input.TreasuryAddress.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.TreasuryAddress == input.TreasuryAddress);
        }

        var symbols = input.Symbols;
        if (!symbols.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                new HashSet<string>(symbols).Select(symbol => (Expression<Func<TreasuryFundIndex, bool>>)(o => o.Symbol == symbol))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.AvailableFunds);
        
        return new Tuple<long, List<TreasuryFundDto>>(count,
            objectMapper.Map<List<TreasuryFundIndex>, List<TreasuryFundDto>>(queryable.ToList()));
    }
    
    [Name("getTreasuryRecordList")]
    public static async Task<List<TreasuryRecordDto>> GetTreasuryRecordListAsync(
        [FromServices] IReadOnlyRepository<TreasuryRecordIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetTreasuryFundListInput input)
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
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DaoId == input.DaoId);
        }
        if (!input.TreasuryAddress.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.TreasuryAddress == input.TreasuryAddress);
        }
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderByDescending(a => a.BlockHeight);
        return objectMapper.Map<List<TreasuryRecordIndex>, List<TreasuryRecordDto>>(queryable.ToList());
    }
}