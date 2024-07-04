using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.Common;

public abstract class StatisticProcessorBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    public async Task TreasuryStatistic(string chainId, string symbol, long deltaAmount, LogEventContext context)
    {
        Logger.LogInformation("TreasuryStatisticBegin chainId {chainId} symbol {symbol} deltaAmount {deltaAmount}",
            chainId, symbol, deltaAmount);
        var fundSumId = IdGenerateHelper.GetId(chainId, symbol);
        var fundSumIndex = await GetEntityAsync<TreasuryFundSumIndex>(fundSumId)
                           ?? new TreasuryFundSumIndex { Id = fundSumId, Symbol = symbol };
        fundSumIndex.AvailableFunds += deltaAmount;
        Logger.LogInformation("TreasuryStatisticEnd chainId {chainId} symbol {symbol} deltaAmount {deltaAmount}",
            chainId, symbol, deltaAmount);
        await SaveEntityAsync(fundSumIndex, context);
    }
}