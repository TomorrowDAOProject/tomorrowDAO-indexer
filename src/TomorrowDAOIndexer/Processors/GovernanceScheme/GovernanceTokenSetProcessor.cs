using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.GovernanceScheme;

public class GovernanceTokenSetProcessor : GovernanceSchemeProcessorBase<GovernanceTokenSet>
{
    public override async Task ProcessAsync(GovernanceTokenSet logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var DAOId = logEvent.DaoId?.ToHex();
        var governanceToken = logEvent.GovernanceToken;
        Logger.LogInformation("[GovernanceTokenSet] start DAOId {DAOId} governanceToken {governanceToken}", DAOId, governanceToken);
        try
        {
            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex != null)
            {
                DAOIndex.GovernanceToken = governanceToken;
                await SaveEntityAsync(DAOIndex, context);
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