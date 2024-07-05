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
        context.Services.AddSingleton<FileInfosRemovedProcessor>();
        context.Services.AddSingleton<FileInfosUploadedProcessor>();
        context.Services.AddSingleton<HighCouncilDisabledProcessor>();
        context.Services.AddSingleton<HighCouncilEnabledProcessor>();
        context.Services.AddSingleton<SubsistStatusSetProcessor>();
        context.Services.AddSingleton<MetadataUpdatedProcessor>();
        context.Services.AddSingleton<MemberAddedProcessor>();
        context.Services.AddSingleton<MemberRemovedProcessor>();
        
        // GovernanceScheme
        context.Services.AddSingleton<GovernanceSchemeAddedProcessor>();
        context.Services.AddSingleton<GovernanceSchemeThresholdRemovedProcessor>();
        context.Services.AddSingleton<GovernanceSchemeThresholdUpdatedProcessor>();
        context.Services.AddSingleton<GovernanceTokenSetProcessor>();
        
        // proposal
        context.Services.AddSingleton<ProposalCreatedProcessor>();
        context.Services.AddSingleton<ProposalExecutedProcessor>();
        context.Services.AddSingleton<ProposalVetoedProcessor>();
        context.Services.AddSingleton<DAOProposalTimePeriodSetProcessor>();
        
        // vote
        context.Services.AddSingleton<VoteSchemeCreatedProcessor>();
        context.Services.AddSingleton<VotingItemRegisteredProcessor>();
        context.Services.AddSingleton<VotedProcessor>();
        context.Services.AddSingleton<VoteWithdrawnProcessor>();
    }
}