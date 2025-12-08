using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Processors.GovernanceScheme;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class DAOProposalTimePeriodSetProcessor : GovernanceProcessorBase<DaoProposalTimePeriodSet>
{
    public override async Task ProcessAsync(DaoProposalTimePeriodSet logEvent, LogEventContext context)
    {
        var chainId = context.ChainId;
        var DAOId = logEvent.DaoId?.ToHex();
        Logger.LogInformation("[DaoProposalTimePeriodSet] start DAOId:{DAOId} chainId:{chainId} ", DAOId, chainId);
        try
        {
            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[DaoProposalTimePeriodSet] DAO not existed DAOId:{DAOId} chainId:{chainId} ", DAOId, chainId);
                return;
            }
            ObjectMapper.Map(logEvent, DAOIndex);
            await SaveEntityAsync(DAOIndex, context);
            Logger.LogInformation("[DaoProposalTimePeriodSet] end DAOId:{DAOId} chainId:{chainId} ", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[DaoProposalTimePeriodSet] Exception DAOId:{DAOId} chainId:{chainId}", DAOId, chainId);
            throw;
        }
    }
}