using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Proposal;

public class DAOProposalTimePeriodSetProcessor : ProposalProcessorBase<DaoProposalTimePeriodSet>
{
    public DAOProposalTimePeriodSetProcessor(ILogger<AElfLogEventProcessorBase<DaoProposalTimePeriodSet, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> proposalRepository,
        IGovernanceProvider governanceProvider, IDAOProvider DAOProvider) :
        base(logger, objectMapper, contractInfoOptions, proposalRepository, governanceProvider, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(DaoProposalTimePeriodSet eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var DAOId = eventValue.DaoId?.ToHex();
        Logger.LogInformation("[DaoProposalTimePeriodSet] start DAOId:{DAOId} chainId:{chainId} ", DAOId, chainId);
        try
        {
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[DaoProposalTimePeriodSet] DAO not existed DAOId:{DAOId} chainId:{chainId} ", DAOId, chainId);
                return;
            }
            ObjectMapper.Map(eventValue, DAOIndex);
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[DaoProposalTimePeriodSet] end DAOId:{DAOId} chainId:{chainId} ", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[DaoProposalTimePeriodSet] Exception DAOId:{DAOId} chainId:{chainId}", DAOId, chainId);
            throw;
        }
    }
}