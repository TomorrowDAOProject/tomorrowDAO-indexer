using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using ProposalStage = TomorrowDAO.Indexer.Plugin.Enums.ProposalStage;
using ProposalStatus = TomorrowDAO.Indexer.Plugin.Enums.ProposalStatus;

namespace TomorrowDAOIndexer.Processors.Proposal;

public class ProposalCreatedProcessor : GovernanceProcessorBase<ProposalCreated>
{
    public override async Task ProcessAsync(ProposalCreated logEvent, LogEventContext context)
    {
       var chainId = context.ChainId;
        var proposalId = logEvent.ProposalId?.ToHex();
        var DAOId = logEvent.DaoId?.ToHex();
        var schemeAddress = logEvent.SchemeAddress?.ToBase58();
        var vetoProposalId = logEvent.VetoProposalId?.ToHex();
        var proposer = logEvent.Proposer?.ToBase58();
        Logger.LogInformation("[ProposalCreated] start proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
        try
        {
            var proposalIndex = await GetEntityAsync<ProposalIndex>(proposalId);
            if (proposalIndex != null)
            {
                Logger.LogInformation("[ProposalCreated] proposalIndex with id {id} chainId {chainId} has existed.", proposalId, chainId);
                return;
            }

            proposalIndex = ObjectMapper.Map<ProposalCreated, ProposalIndex>(logEvent);
            var governanceScheme = await GetEntityAsync<GovernanceSchemeIndex>(IdGenerateHelper.GetId(chainId, DAOId, schemeAddress));
            if (governanceScheme != null)
            { 
                ObjectMapper.Map(governanceScheme, proposalIndex);
            }
            var DAO = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAO != null)
            { 
                ObjectMapper.Map(DAO, proposalIndex);
            }
            proposalIndex.DeployTime = context.Block.BlockTime;
            proposalIndex.Id = proposalId;
            await SaveEntityAsync(proposalIndex, context);

            if (vetoProposalId != null)
            {
                UpdateProposal(vetoProposalId, ProposalStatus.Challenged, ProposalStage.Pending, string.Empty, proposalId, context);
            }

            await SaveEntityAsync(new LatestParticipatedIndex
            {
                Id = IdGenerateHelper.GetId(chainId, proposer),
                DAOId = DAOId, Address = proposer,
                ParticipatedType = ParticipatedType.Proposed,
                LatestParticipatedTime = context.Block.BlockTime
            }, context);
            Logger.LogInformation("[ProposalCreated] end proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ProposalCreated] Exception proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
            throw;
        }
    }
}