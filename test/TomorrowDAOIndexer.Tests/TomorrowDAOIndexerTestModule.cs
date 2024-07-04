using AeFinder.App.TestBase;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAOIndexer.Processors;
using TomorrowDAOIndexer.Processors.DAO;
using TomorrowDAOIndexer.Processors.GovernanceScheme;
using TomorrowDAOIndexer.Processors.Proposal;
using TomorrowDAOIndexer.Processors.Vote;
using Volo.Abp.Modularity;

namespace TomorrowDAOIndexer;

[DependsOn(
    typeof(AeFinderAppTestBaseModule),
    typeof(TomorrowDAOIndexerModule))]
public class TomorrowDAOIndexerTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AeFinderAppEntityOptions>(options => { options.AddTypes<TomorrowDAOIndexerModule>(); });
        
        // DAO
        context.Services.AddSingleton<DAOCreatedProcessor>();
        context.Services.AddTransient<FileInfosRemovedProcessor>();
        context.Services.AddTransient<FileInfosUploadedProcessor>();
        context.Services.AddTransient<HighCouncilDisabledProcessor>();
        context.Services.AddTransient<HighCouncilEnabledProcessor>();
        context.Services.AddTransient<SubsistStatusSetProcessor>();
        context.Services.AddTransient<MetadataUpdatedProcessor>();
        context.Services.AddTransient<MemberAddedProcessor>();
        context.Services.AddTransient<MemberRemovedProcessor>();
        
        // GovernanceScheme
        context.Services.AddSingleton<GovernanceSchemeAddedProcessor>();
        context.Services.AddTransient<GovernanceSchemeThresholdRemovedProcessor>();
        context.Services.AddTransient<GovernanceSchemeThresholdUpdatedProcessor>();
        context.Services.AddTransient<GovernanceTokenSetProcessor>();
        
        // proposal
        context.Services.AddSingleton<ProposalCreatedProcessor>();
        context.Services.AddTransient<ProposalExecutedProcessor>();
        context.Services.AddTransient<ProposalVetoedProcessor>();
        context.Services.AddTransient<DAOProposalTimePeriodSetProcessor>();
        
        // vote
        context.Services.AddSingleton<VoteSchemeCreatedProcessor>();
        context.Services.AddSingleton<VotingItemRegisteredProcessor>();
        context.Services.AddSingleton<VotedProcessor>();
        context.Services.AddSingleton<VoteWithdrawnProcessor>();
    }
}