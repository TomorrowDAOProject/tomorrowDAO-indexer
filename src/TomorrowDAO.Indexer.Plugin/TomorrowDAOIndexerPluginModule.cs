using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Election;
using TomorrowDAO.Indexer.Plugin.Processors.Proposal;
using TomorrowDAO.Indexer.Plugin.Processors.Treasury;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Vote = TomorrowDAO.Indexer.Plugin.Processors.Vote;

namespace TomorrowDAO.Indexer.Plugin;

[DependsOn(typeof(AElfIndexerClientModule), typeof(AbpAutoMapperModule))]
public class TomorrowDAOIndexerPluginModule : AElfIndexerClientPluginBaseModule<TomorrowDAOIndexerPluginModule,
    TomorrowDAOIndexerPluginSchema, Query>
{
    protected override void ConfigureServices(IServiceCollection serviceCollection)
    {
        var configuration = serviceCollection.GetConfiguration();
        Configure<ContractInfoOptions>(configuration.GetSection("ContractInfo"));
        // proposal
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ProposalCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ProposalExecutedProcessor>();
        // DAO
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, DAOCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, FileInfosRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, FileInfosUploadedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilDisabledProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilEnabledProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, PausedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SubsistStatusSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, UnpausedProcessor>();
        // election
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddressReplacedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateInfoUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, VotedProcessor>();
        // treasury
        // organization
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, OrganizationCreatedProcessor>();
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberAddedProcessor>();
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberChangedProcessor>();
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberRemovedProcessor>();
        // governanceScheme
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSchemeCreatedProcessor>();
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSubSchemeAddedProcessor>();
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSubSchemeRemovedProcessor>();
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSubSchemeUpdatedProcessor>();
        // serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceThresholdUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, DonationReceivedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, EmergencyTransferredProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SupportedStakingTokensAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SupportedStakingTokensRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TokenStakedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryTokenLockedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryTokenUnlockedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryTransferReleasedProcessor>();
        // vote
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, Vote.VoteCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, Vote.VotingItemRegisteredProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, Vote.VotedProcessor>();
    }

    protected override string ClientId => "AElfIndexer_TomorrowDAO";
    protected override string Version => "3a6d3abe5e4f4356a02c35a6d90257e7";
}