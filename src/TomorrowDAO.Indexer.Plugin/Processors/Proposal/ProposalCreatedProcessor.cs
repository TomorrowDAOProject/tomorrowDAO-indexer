using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using ProposalStage = TomorrowDAO.Indexer.Plugin.Enums.ProposalStage;
using ProposalStatus = TomorrowDAO.Indexer.Plugin.Enums.ProposalStatus;

namespace TomorrowDAO.Indexer.Plugin.Processors.Proposal;

public class ProposalCreatedProcessor : ProposalProcessorBase<ProposalCreated>
{
    public ProposalCreatedProcessor(ILogger<AElfLogEventProcessorBase<ProposalCreated, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> proposalRepository,
        IGovernanceProvider governanceProvider, IDAOProvider DAOProvider) :
        base(logger, objectMapper, contractInfoOptions, proposalRepository, governanceProvider, DAOProvider)
    {
    }

    protected override async Task HandleEventAsync(ProposalCreated eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId?.ToHex();
        var DAOId = eventValue.DaoId?.ToHex();
        var schemeAddress = eventValue.SchemeAddress?.ToBase58();
        var vetoProposalId = eventValue.VetoProposalId?.ToHex();
        Logger.LogInformation("[ProposalCreated] start proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
        try
        {
            var proposalIndex = await ProposalRepository.GetFromBlockStateSetAsync(proposalId, chainId);
            if (proposalIndex != null)
            {
                Logger.LogInformation("[ProposalCreated] proposalIndex with id {id} chainId {chainId} has existed.", proposalId, chainId);
                return;
            }

            proposalIndex = ObjectMapper.Map<ProposalCreated, ProposalIndex>(eventValue);
            var governanceScheme = await GovernanceProvider.GetGovernanceSchemeAsync(chainId, IdGenerateHelper.GetId(chainId, DAOId, schemeAddress));
            if (governanceScheme != null)
            { 
                ObjectMapper.Map(governanceScheme, proposalIndex);
            }
            var DAO = await DAOProvider.GetDAOAsync(chainId, DAOId);
            if (DAO != null)
            { 
                ObjectMapper.Map(DAO, proposalIndex);
            }
            proposalIndex.DeployTime = context.BlockTime;
            proposalIndex.Id = proposalId;
            await SaveIndexAsync(proposalIndex, context);

            if (vetoProposalId != null)
            {
                UpdateVetoProposal(vetoProposalId, ProposalStatus.Challenged, ProposalStage.Pending, context);
            }
            Logger.LogInformation("[ProposalCreated] end proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ProposalCreated] Exception proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
            throw;
        }
    }
}