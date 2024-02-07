using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Election;
using TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;
using TomorrowDAO.Indexer.Plugin.Processors.Organization;
using TomorrowDAO.Indexer.Plugin.Processors.Proposal;
using TomorrowDAO.Indexer.Plugin.Processors.Treasury;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

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
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, PermissionsSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SubsistStatusSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryContractSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, UnpausedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, VoteContractSetProcessor>();
        // election
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddressReplacedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateInfoUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateRemovedProcessor>();
        // treasury
        // organization
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, OrganizationCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberChangedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberRemovedProcessor>();
        // governanceScheme
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSchemeCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSubSchemeAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSubSchemeRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSubSchemeUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceThresholdUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, DonationReceivedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, EmergencyTransferredProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SupportedStakingTokensAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SupportedStakingTokensRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TokenStakedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryTokenLockedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryTokenUnlockedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryTransferReleasedProcessor>();
    }

    protected override string ClientId => "AElfIndexer_tomorrowDAO";
    protected override string Version => "******";
}