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
        context.Services.AddTransient<ILogEventProcessor, CommittedProcessor>();
        
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
        
        // token
        context.Services.AddTransient<ILogEventProcessor, IssuedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, BurnedProcessor>();
    }
}