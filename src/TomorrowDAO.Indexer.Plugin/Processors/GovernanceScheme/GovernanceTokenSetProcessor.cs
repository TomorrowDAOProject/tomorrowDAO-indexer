using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;

public class GovernanceTokenSetProcessor : GovernanceSchemeProcessorBase<GovernanceTokenSet>
{
    public GovernanceTokenSetProcessor(
        ILogger<AElfLogEventProcessorBase<GovernanceTokenSet, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<GovernanceSchemeIndex, LogEventInfo> governanceSchemeRepository,
        IDAOProvider DAOProvider) :
        base(logger, objectMapper, contractInfoOptions, governanceSchemeRepository, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(GovernanceTokenSet eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var DAOId = eventValue.DaoId?.ToHex();
        var governanceToken = eventValue.GovernanceToken;
        Logger.LogInformation("[GovernanceTokenSet] start DAOId {DAOId} governanceToken {governanceToken}", DAOId, governanceToken);
        try
        {
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            if (DAOIndex != null)
            {
                DAOIndex.GovernanceToken = governanceToken;
                await DAOProvider.SaveIndexAsync(DAOIndex, context);
            }
            
            var mustQuery = new List<Func<QueryContainerDescriptor<GovernanceSchemeIndex>, QueryContainer>>
            {
                q => q.Term(i
                    => i.Field(f => f.ChainId).Value(chainId)),
                q => q.Terms(i
                    => i.Field(f => f.DAOId).Terms(DAOId))
            };
            QueryContainer Filter(QueryContainerDescriptor<GovernanceSchemeIndex> f) =>
                f.Bool(b => b.Must(mustQuery));
            
            var (_, governanceSchemeIndexList) = await GovernanceSchemeRepository.GetListAsync(Filter);
            if (!governanceSchemeIndexList.IsNullOrEmpty())
            {
                foreach (var governanceSchemeIndex in governanceSchemeIndexList)
                {
                    governanceSchemeIndex.GovernanceToken = governanceToken;
                    await SaveIndexAsync(governanceSchemeIndex, context);
                }
            }
            
            Logger.LogInformation("[GovernanceTokenSet] end DAOId {DAOId} governanceToken {governanceToken}", DAOId, governanceToken);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[GovernanceTokenSet] Exception DAOId {DAOId} governanceToken {governanceToken}", DAOId, governanceToken);
            throw;
        }
    }
}