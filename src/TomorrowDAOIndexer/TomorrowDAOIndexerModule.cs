using AeFinder.Sdk.Processor;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAOIndexer.GraphQL;
using TomorrowDAOIndexer.Processors.DAO;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using TomorrowDAOIndexer.Processors.Proposal;
using TomorrowDAOIndexer.Processors.Vote;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

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
    }
}