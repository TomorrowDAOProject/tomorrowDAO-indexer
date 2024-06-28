using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Provider;

public interface ITreasuryProvider
{
    Task SaveIndexAsync(TreasuryFundSumIndex index, LogEventContext context);
    Task TreasuryStatistic(string chainId, string symbol, long deltaAmount, LogEventContext context);
}

public class TreasuryProvider : ITreasuryProvider, ISingletonDependency
{
    private readonly ILogger<TreasuryProvider> _logger;
    private readonly IAElfIndexerClientEntityRepository<TreasuryFundSumIndex, LogEventInfo> _treasuryFundSumRepository;
    private readonly IObjectMapper _objectMapper;

    public TreasuryProvider(IAElfIndexerClientEntityRepository<TreasuryFundSumIndex, LogEventInfo> treasuryFundSumRepository, 
        ILogger<TreasuryProvider> logger, IObjectMapper objectMapper)
    {
        _treasuryFundSumRepository = treasuryFundSumRepository;
        _logger = logger;
        _objectMapper = objectMapper;
    }

    public async Task SaveIndexAsync(TreasuryFundSumIndex index, LogEventContext context)
    {
        _objectMapper.Map(context, index);
        await _treasuryFundSumRepository.AddOrUpdateAsync(index);
    }

    public async Task TreasuryStatistic(string chainId, string symbol, long deltaAmount, LogEventContext context)
    {
        _logger.LogInformation("TreasuryStatisticBegin chainId {chainId} symbol {symbol} deltaAmount {deltaAmount}", 
            chainId, symbol, deltaAmount);
        var fundSumId = IdGenerateHelper.GetId(chainId, symbol);
        var fundSumIndex = await _treasuryFundSumRepository.GetFromBlockStateSetAsync(fundSumId, chainId) 
                           ?? new TreasuryFundSumIndex { Id = fundSumId, Symbol = symbol};
        fundSumIndex.AvailableFunds += deltaAmount;
        _logger.LogInformation("TreasuryStatisticEnd chainId {chainId} symbol {symbol} deltaAmount {deltaAmount}", 
            chainId, symbol, deltaAmount);
        await SaveIndexAsync(fundSumIndex, context);
    }
}