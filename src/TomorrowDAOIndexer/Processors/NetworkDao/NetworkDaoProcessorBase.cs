using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.Referendum;
using AElf.CSharp.Core;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using Transaction = TomorrowDAOIndexer.Entities.Transaction;

namespace TomorrowDAOIndexer.Processors.NetworkDao;

public abstract class NetworkDaoProposalBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected async Task SaveProposalIndexAsync(ProposalCreated eventValue, LogEventContext context,
        NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId?.ToHex();
        Logger.LogInformation("[ACS3.ProposalCreated] start. chainId={ChainId}, proposalId={ProposalId}", chainId,
            proposalId);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, proposalId);
            var proposalIndex = await GetEntityAsync<NetworkDaoProposalIndex>(id);
            if (proposalIndex != null)
            {
                Logger.LogError("[ACS3.ProposalCreated] Network DAO ProposalIndex already existed id {id}", id);
                return;
            }

            proposalIndex = ObjectMapper.Map<ProposalCreated, NetworkDaoProposalIndex>(eventValue);
            proposalIndex.Id = id;
            proposalIndex.OrgType = orgType;
            proposalIndex.SaveTime = DateTime.Now;
            await SaveEntityAsync(proposalIndex, context);
            Logger.LogInformation("[ACS3.ProposalCreated] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.ProposalCreated] Exception. chainId={ChainId}, proposalId={ProposalId}", chainId,
                proposalId);
            throw;
        }
    }

    protected async Task SaveProposalReleasedIndexAsync(ProposalReleased eventValue, LogEventContext context,
        NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId?.ToHex();
        Logger.LogInformation("[ACS3.ProposalReleased] start. chainId={ChainId}, proposalId={ProposalId}", chainId,
            proposalId);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, proposalId);
            var proposalReleasedIndex = await GetEntityAsync<NetworkDaoProposalReleasedIndex>(id);
            if (proposalReleasedIndex != null)
            {
                Logger.LogError("[ACS3.ProposalReleased] Network DAO ProposalReleasedIndex already existed id {id}",
                    id);
                return;
            }

            proposalReleasedIndex = ObjectMapper.Map<ProposalReleased, NetworkDaoProposalReleasedIndex>(eventValue);
            proposalReleasedIndex.Id = id;
            proposalReleasedIndex.OrgType = orgType;
            await SaveEntityAsync(proposalReleasedIndex, context);

            await UpdateProposalIndexStatusAsync(chainId, proposalId, context);

            Logger.LogInformation("[ACS3.ProposalReleased] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.ProposalReleased] Exception. chainId={ChainId}, proposalId={ProposalId}", chainId,
                proposalId);
            throw;
        }
    }

    protected async Task SaveReceiptCreatedAsync(ReferendumReceiptCreated logEvent, LogEventContext context,
        NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var proposalId = logEvent.ProposalId?.ToHex();
        var transactionId = context.Transaction.TransactionId;
        Logger.LogInformation($"[ACS3.ReceiptCreated] start. chainId={0},proposalId={1},transactionId={2}", chainId,
            proposalId, transactionId);

        try
        {
            var id = IdGenerateHelper.GetId(chainId, transactionId);
            var voteIndex = await GetEntityAsync<NetworkDaoProposalVoteRecordIndex>(id);
            if (voteIndex != null)
            {
                Logger.LogError("[ACS3.ReceiptCreated] Network DAO ReceiptCreated already existed id {id}",
                    id);
                return;
            }

            voteIndex = ObjectMapper.Map<ReferendumReceiptCreated, NetworkDaoProposalVoteRecordIndex>(logEvent);
            voteIndex.Id = id;
            voteIndex.OrgType = orgType;
            await SaveEntityAsync(voteIndex, context);

            await UpdateProposalIndexAmountAsync(chainId, proposalId, voteIndex.Symbol, voteIndex.Amount, context);

            Logger.LogInformation("[ACS3.ReceiptCreated] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.ReceiptCreated] Exception. chainId={ChainId}, proposalId={ProposalId}", chainId,
                proposalId);
            throw;
        }
    }

    protected async Task SaveOrgCreatedIndexAsync(OrganizationCreated logEvent, LogEventContext context,
        NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var organizationAddress = logEvent.OrganizationAddress?.ToBase58();
        Logger.LogInformation("[ACS3.OrganizationCreated] start. chainId={0},orgAddress={1}", chainId,
            organizationAddress);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, organizationAddress);
            var orgCreatedIndex = await GetEntityAsync<NetworkDaoOrgCreatedIndex>(id);
            if (orgCreatedIndex != null)
            {
                Logger.LogError("[ACS3.OrganizationCreated] Network DAO OrganizationCreated already existed id {id}",
                    id);
                return;
            }

            orgCreatedIndex = ObjectMapper.Map<OrganizationCreated, NetworkDaoOrgCreatedIndex>(logEvent);
            orgCreatedIndex.Id = id;
            orgCreatedIndex.OrgType = orgType;
            orgCreatedIndex.Transaction =
                ObjectMapper.Map<AeFinder.Sdk.Processor.Transaction, Transaction>(context.Transaction);
            await SaveEntityAsync(orgCreatedIndex, context);

            await UpdateOrgChangedIndexAsync(chainId, organizationAddress, orgType, context);

            Logger.LogInformation("[ACS3.OrganizationCreated] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.OrganizationCreated] Exception. chainId={0},orgAddress={1}", chainId,
                organizationAddress);
            throw;
        }
    }

    protected async Task SaveOrgWhiteListChangedIndexAsync(OrganizationWhiteListChanged logEvent,
        LogEventContext context, NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var organizationAddress = logEvent.OrganizationAddress?.ToBase58();
        var transactionId = context.Transaction.TransactionId;
        Logger.LogInformation("[ACS3.OrganizationWhiteListChanged] start. chainId={0},orgAddress={1}", chainId,
            organizationAddress);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, transactionId);
            var orgWhiteListChangedIndex = await GetEntityAsync<NetworkDaoOrgWhiteListChangedIndex>(id);
            if (orgWhiteListChangedIndex != null)
            {
                Logger.LogError(
                    "[ACS3.OrganizationWhiteListChanged] Network DAO OrgWhiteListChanged already existed id {id}",
                    id);
                return;
            }

            orgWhiteListChangedIndex =
                ObjectMapper.Map<OrganizationWhiteListChanged, NetworkDaoOrgWhiteListChangedIndex>(logEvent);
            orgWhiteListChangedIndex.Id = id;
            orgWhiteListChangedIndex.OrgType = orgType;
            await SaveEntityAsync(orgWhiteListChangedIndex, context);

            await UpdateOrgChangedIndexAsync(chainId, organizationAddress, orgType, context);

            Logger.LogInformation("[ACS3.OrganizationWhiteListChanged] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.OrganizationWhiteListChanged] Exception. chainId={0},orgAddress={1}", chainId,
                organizationAddress);
            throw;
        }
    }

    protected async Task SaveOrgThresholdChangedIndexAsync(OrganizationThresholdChanged logEvent,
        LogEventContext context, NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var organizationAddress = logEvent.OrganizationAddress?.ToBase58();
        var transactionId = context.Transaction.TransactionId;
        Logger.LogInformation("[ACS3.OrganizationThresholdChanged] start. chainId={0},orgAddress={1}", chainId,
            organizationAddress);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, transactionId);
            var orgThresholdChangedIndex = await GetEntityAsync<NetworkDaoOrgThresholdChangedIndex>(id);
            if (orgThresholdChangedIndex != null)
            {
                Logger.LogError(
                    "[ACS3.OrganizationThresholdChanged] Network DAO OrgThresholdChangedIndex already existed id {id}",
                    id);
                return;
            }

            orgThresholdChangedIndex =
                ObjectMapper.Map<OrganizationThresholdChanged, NetworkDaoOrgThresholdChangedIndex>(logEvent);
            orgThresholdChangedIndex.Id = id;
            orgThresholdChangedIndex.OrgType = orgType;
            await SaveEntityAsync(orgThresholdChangedIndex, context);

            await UpdateOrgChangedIndexAsync(chainId, organizationAddress, orgType, context);

            Logger.LogInformation("[ACS3.OrganizationThresholdChanged] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.OrganizationThresholdChanged] Exception. chainId={0},orgAddress={1}", chainId,
                organizationAddress);
            throw;
        }
    }
    
    protected async Task SaveOrgMemberAddedIndexAsync(AElf.Contracts.Association.MemberAdded logEvent,
        LogEventContext context, NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var organizationAddress = logEvent.OrganizationAddress?.ToBase58();
        var transactionId = context.Transaction.TransactionId;
        Logger.LogInformation("[{0}.MemberAdded] start. chainId={1},orgAddress={2}", orgType.ToString(), chainId,
            organizationAddress);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, transactionId);
            var orgMemberChangedIndex = await GetEntityAsync<NetworkDaoOrgMemberChangedIndex>(id);
            if (orgMemberChangedIndex != null)
            {
                Logger.LogError(
                    "[{0}.MemberAdded] Network DAO OrgMemberChangedIndex already existed id {1}",
                    orgType.ToString(), id);
                return;
            }

            orgMemberChangedIndex =
                ObjectMapper.Map<AElf.Contracts.Association.MemberAdded, NetworkDaoOrgMemberChangedIndex>(logEvent);
            orgMemberChangedIndex.Id = id;
            orgMemberChangedIndex.OrgType = orgType;
            orgMemberChangedIndex.ChangeType = OrgMemberChangeTypeEnum.MemberAdded;
            await SaveEntityAsync(orgMemberChangedIndex, context);

            await UpdateOrgChangedIndexAsync(chainId, organizationAddress, orgType, context);

            Logger.LogInformation("[{0}.MemberAdded] Finished. id {1}", orgType.ToString(), id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[{0}.MemberAdded] Exception. chainId={1},orgAddress={2}", orgType.ToString(), chainId,
                organizationAddress);
            throw;
        }
    }
    
    protected async Task SaveOrgMemberRemovedIndexAsync(AElf.Contracts.Association.MemberRemoved logEvent,
        LogEventContext context, NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var organizationAddress = logEvent.OrganizationAddress?.ToBase58();
        var transactionId = context.Transaction.TransactionId;
        Logger.LogInformation("[{0}.MemberRemoved] start. chainId={1},orgAddress={2}", orgType.ToString(), chainId,
            organizationAddress);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, transactionId);
            var orgMemberChangedIndex = await GetEntityAsync<NetworkDaoOrgMemberChangedIndex>(id);
            if (orgMemberChangedIndex != null)
            {
                Logger.LogError(
                    "[{0}.MemberRemoved] Network DAO OrgMemberChangedIndex already existed id {1}",
                    orgType.ToString(), id);
                return;
            }

            orgMemberChangedIndex =
                ObjectMapper.Map<AElf.Contracts.Association.MemberRemoved, NetworkDaoOrgMemberChangedIndex>(logEvent);
            orgMemberChangedIndex.Id = id;
            orgMemberChangedIndex.OrgType = orgType;
            orgMemberChangedIndex.ChangeType = OrgMemberChangeTypeEnum.MemberRemoved;
            await SaveEntityAsync(orgMemberChangedIndex, context);

            await UpdateOrgChangedIndexAsync(chainId, organizationAddress, orgType, context);

            Logger.LogInformation("[{0}.MemberRemoved] Finished. id {1}", orgType.ToString(), id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[{0}.MemberRemoved] Exception. chainId={1},orgAddress={2}", orgType.ToString(), chainId,
                organizationAddress);
            throw;
        }
    }

    protected async Task SaveOrgMemberChangedIndexAsync(AElf.Contracts.Association.MemberChanged logEvent,
        LogEventContext context, NetworkDaoOrgType orgType)
    {
        var chainId = context.ChainId;
        var organizationAddress = logEvent.OrganizationAddress?.ToBase58();
        var transactionId = context.Transaction.TransactionId;
        Logger.LogInformation("[{0}.MemberChanged] start. chainId={1},orgAddress={2}", orgType.ToString(), chainId,
            organizationAddress);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, transactionId);
            var orgMemberChangedIndex = await GetEntityAsync<NetworkDaoOrgMemberChangedIndex>(id);
            if (orgMemberChangedIndex != null)
            {
                Logger.LogError(
                    "[{0}.MemberChanged] Network DAO OrgMemberChangedIndex already existed id {1}",
                    orgType.ToString(), id);
                return;
            }

            orgMemberChangedIndex =
                ObjectMapper.Map<AElf.Contracts.Association.MemberChanged, NetworkDaoOrgMemberChangedIndex>(logEvent);
            orgMemberChangedIndex.Id = id;
            orgMemberChangedIndex.OrgType = orgType;
            orgMemberChangedIndex.ChangeType = OrgMemberChangeTypeEnum.MemberChanged;
            await SaveEntityAsync(orgMemberChangedIndex, context);

            await UpdateOrgChangedIndexAsync(chainId, organizationAddress, orgType, context);

            Logger.LogInformation("[{0}.MemberChanged] Finished. id {1}", orgType.ToString(), id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[{0}.MemberChanged] Exception. chainId={1},orgAddress={2}", orgType.ToString(), chainId,
                organizationAddress);
            throw;
        }
    }

    private async Task UpdateProposalIndexStatusAsync(string chainId, string proposalId, LogEventContext context)
    {
        Logger.LogInformation("[ACS3.ProposalReleased] Update ProposalIndex Status, chainId={0},proposalId={1}",
            chainId,
            proposalId);
        var id = IdGenerateHelper.GetId(chainId, proposalId);
        var proposalIndex = await GetEntityAsync<NetworkDaoProposalIndex>(id);
        if (proposalIndex == null)
        {
            Logger.LogError("[ACS3.ProposalReleased] Update ProposalIndex Status error, index not exist. id {id}", id);
            return;
        }

        proposalIndex.IsReleased = true;
        await SaveEntityAsync(proposalIndex, context);
        Logger.LogInformation(
            "[ACS3.ProposalReleased] Update ProposalIndex Status finished, chainId={0},proposalId={1}",
            chainId, proposalId);
    }

    private async Task UpdateProposalIndexAmountAsync(string chainId, string? proposalId, string voteIndexSymbol,
        long voteIndexAmount, LogEventContext context)
    {
        Logger.LogInformation("[ACS3.ReceiptCreated] Update ProposalIndex Amount start.proposalId={0}", proposalId);
        var id = IdGenerateHelper.GetId(chainId, proposalId);
        var proposalIndex = await GetEntityAsync<NetworkDaoProposalIndex>(id);
        if (proposalIndex == null)
        {
            Logger.LogError("[ACS3.ProposalReleased] Update ProposalIndex Amount error, index not exist. id {id}", id);
            return;
        }

        proposalIndex.TotalAmount += voteIndexAmount;
        proposalIndex.Symbol = voteIndexSymbol;
        await SaveEntityAsync(proposalIndex, context);
        Logger.LogInformation(
            "[ACS3.ProposalReleased] Update ProposalIndex Amount finished, chainId={0},proposalId={1}",
            chainId, proposalId);
    }

    private async Task UpdateOrgChangedIndexAsync(string chainId, string orgAddress, NetworkDaoOrgType orgType,
        LogEventContext context)
    {
        Logger.LogInformation("[ACS3.Organization] Update OrgChangedIndex start. orgAddress={0}", orgAddress);
        var id = IdGenerateHelper.GetId(chainId, orgAddress);
        var orgChangedIndex = await GetEntityAsync<NetworkDaoOrgChangedIndex>(id);
        if (orgChangedIndex == null)
        {
            orgChangedIndex = new NetworkDaoOrgChangedIndex
            {
                Id = id,
                OrganizationAddress = orgAddress,
                OrgType = orgType
            };
        }

        await SaveEntityAsync(orgChangedIndex, context);
        Logger.LogInformation(
            "[ACS3.Organization] Update OrgChangedIndex finished, chainId={0}, orgAddress={1}",
            chainId, orgAddress);
    }
}