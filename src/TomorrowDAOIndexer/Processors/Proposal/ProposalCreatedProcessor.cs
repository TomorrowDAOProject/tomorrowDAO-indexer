using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using ProposalStage = TomorrowDAO.Indexer.Plugin.Enums.ProposalStage;
using ProposalStatus = TomorrowDAOIndexer.Enums.ProposalStatus;

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
        Logger.LogInformation("[ProposalCreated] start proposalId:{proposalId} chainId:{chainId} ", proposalId,
            chainId);
        try
        {
            var proposalIndex = await GetEntityAsync<ProposalIndex>(proposalId);
            if (proposalIndex != null)
            {
                Logger.LogInformation("[ProposalCreated] proposalIndex with id {id} chainId {chainId} has existed.",
                    proposalId, chainId);
                return;
            }

            proposalIndex = ObjectMapper.Map<ProposalCreated, ProposalIndex>(logEvent);
            var DAO = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAO != null)
            {
                ObjectMapper.Map(DAO, proposalIndex);
                await UpdateDaoProposalCountAsync(DAO, context);
            }

            var governanceScheme =
                await GetEntityAsync<GovernanceSchemeIndex>(IdGenerateHelper.GetId(chainId, DAOId, schemeAddress));
            if (governanceScheme != null)
            {
                ObjectMapper.Map(governanceScheme, proposalIndex);
            }

            proposalIndex.DeployTime = context.Block.BlockTime;
            proposalIndex.Id = proposalId;
            await SaveEntityAsync(proposalIndex, context);

            if (vetoProposalId != null)
            {
                await UpdateProposal(vetoProposalId, ProposalStatus.Challenged, ProposalStage.Pending, string.Empty,
                    proposalId, context);
            }

            await SaveEntityAsync(new LatestParticipatedIndex
            {
                Id = IdGenerateHelper.GetId(chainId, proposer),
                DAOId = DAOId, Address = proposer,
                ParticipatedType = ParticipatedType.Proposed,
                LatestParticipatedTime = context.Block.BlockTime
            }, context);
            Logger.LogInformation("[ProposalCreated] end proposalId:{proposalId} chainId:{chainId} ", proposalId,
                chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ProposalCreated] Exception proposalId:{proposalId} chainId:{chainId} ", proposalId,
                chainId);
            throw;
        }
    }

    private async Task UpdateDaoProposalCountAsync(DAOIndex daoIndex, LogEventContext context)
    {
        Logger.LogInformation("[ProposalCreated] Update DAO proposal count, DaoId={0}, ProposalCount={1}",
            daoIndex.Id, daoIndex.ProposalCount);
        try
        {
            daoIndex.ProposalCount += 1;
            await SaveEntityAsync(daoIndex, context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ProposalCreated] Update DAO proposal count error, DaoId={0}, ProposalCount={1}",
                daoIndex.Id, daoIndex.ProposalCount);
        }
    }
}