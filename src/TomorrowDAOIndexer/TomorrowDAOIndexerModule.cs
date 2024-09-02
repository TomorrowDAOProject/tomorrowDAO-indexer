using AeFinder.Sdk.Processor;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAOIndexer.GraphQL;
using TomorrowDAOIndexer.Processors.DAO;
using TomorrowDAOIndexer.Processors.Election;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using TomorrowDAOIndexer.Processors.NetworkDao.Association;
using TomorrowDAOIndexer.Processors.NetworkDao.Parliament;
using TomorrowDAOIndexer.Processors.NetworkDao.Referendum;
using TomorrowDAOIndexer.Processors.Proposal;
using TomorrowDAOIndexer.Processors.Token;
using TomorrowDAOIndexer.Processors.TokenConverter;
using TomorrowDAOIndexer.Processors.Treasury;
using TomorrowDAOIndexer.Processors.Vote;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using VotedProcessor = TomorrowDAOIndexer.Processors.Vote.VotedProcessor;

namespace TomorrowDAOIndexer;

public class TomorrowDAOIndexerModule: AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options => { options.AddMaps<TomorrowDAOIndexerModule>(); });
        context.Services.AddSingleton<ISchema, AppSchema>();
        
        // TokenConverter
        context.Services.AddTransient<ILogEventProcessor, TokenSoldProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TokenBoughtProcessor>();
        
        // DAO
        context.Services.AddTransient<ILogEventProcessor, DAOCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, FileInfosRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, FileInfosUploadedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, HighCouncilDisabledProcessor>();
        context.Services.AddTransient<ILogEventProcessor, HighCouncilEnabledProcessor>();
        context.Services.AddTransient<ILogEventProcessor, SubsistStatusSetProcessor>();
        context.Services.AddTransient<ILogEventProcessor, MetadataUpdatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, MemberAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, MemberRemovedProcessor>();
        
        // GovernanceScheme
        context.Services.AddTransient<ILogEventProcessor, GovernanceSchemeAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, GovernanceSchemeThresholdRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, GovernanceSchemeThresholdUpdatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, GovernanceTokenSetProcessor>();
        
        // proposal
        context.Services.AddTransient<ILogEventProcessor, ProposalCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ProposalExecutedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ProposalVetoedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, DAOProposalTimePeriodSetProcessor>();
        
        // vote
        context.Services.AddTransient<ILogEventProcessor, VoteSchemeCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, VotingItemRegisteredProcessor>();
        context.Services.AddTransient<ILogEventProcessor, VotedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, VoteWithdrawnProcessor>();
        
        //Election
        context.Services.AddTransient<ILogEventProcessor, CandidateAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, CandidateAddressReplacedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, CandidateInfoUpdatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, CandidateRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TomorrowDAOIndexer.Processors.Election.VotedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ElectionVotingEventRegisteredProcessor>();
        context.Services.AddTransient<ILogEventProcessor, HighCouncilAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, HighCouncilRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, CandidateElectedProcessor>();
        
        // treasury
        context.Services.AddTransient<ILogEventProcessor, TransferredProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TreasuryCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, TreasuryTransferredProcessor>();
        
        //Network DAO
        context.Services.AddTransient<ILogEventProcessor, AssociationProposalCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ReferendumProposalCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ParliamentProposalCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationOrgCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationOrgMemberAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationOrgMemberChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationOrgMemberRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationOrgThresholdChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationOrgWhiteListChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationProposalReleasedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, AssociationReceiptCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ParliamentOrgCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ParliamentOrgThresholdChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ParliamentOrgWhiteListChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ParliamentProposalReleasedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ParliamentReceiptCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ReferendumOrgCreatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ReferendumOrgThresholdChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ReferendumOrgWhiteListChangedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ReferendumProposalReleasedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, ReferendumReceiptCreatedProcessor>();
        // context.Services.AddTransient<ILogEventProcessor, NetworkDaoTransferredProcessor>();
        
        // Transaction
        // context.Services.AddSingleton<ITransactionProcessor, TransactionProcessor>();
    }
}