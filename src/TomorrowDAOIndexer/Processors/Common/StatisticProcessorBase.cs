using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.Common;

public abstract class StatisticProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected async Task TreasuryFundSumStatistic(string chainId, string symbol, long deltaAmount, LogEventContext context)
    { 
        var fundSumId = IdGenerateHelper.GetId(chainId, symbol);
        var fundSumIndex = await GetEntityAsync<TreasuryFundSumIndex>(fundSumId)
                           ?? new TreasuryFundSumIndex { Id = fundSumId, Symbol = symbol };
        fundSumIndex.AvailableFunds += deltaAmount;
        await SaveEntityAsync(fundSumIndex, context);
    }
    
    protected async Task TreasuryFundStatistic(string chainId, string daoId, string symbol,
        string address, long deltaAmount, LogEventContext context)
    {
        var id = IdGenerateHelper.GetId(chainId, daoId, symbol);
        var treasuryFundIndex = await GetEntityAsync<TreasuryFundIndex>(id) ?? new TreasuryFundIndex
        {
            Id = id,
            DaoId = daoId,
            TreasuryAddress = address,
            Symbol = symbol
        };
        treasuryFundIndex.AvailableFunds += deltaAmount;
        await SaveEntityAsync(treasuryFundIndex, context);
    }
}