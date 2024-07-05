using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.Processors.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Election;
using TomorrowDAO.Indexer.Plugin.Processors.GovernanceScheme;
using TomorrowDAO.Indexer.Plugin.Processors.NetworkDao.Association;
using TomorrowDAO.Indexer.Plugin.Processors.NetworkDao.Parliament;
using TomorrowDAO.Indexer.Plugin.Processors.NetworkDao.Referendum;
using TomorrowDAO.Indexer.Plugin.Processors.Proposal;
using TomorrowDAO.Indexer.Plugin.Processors.Token;
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
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ProposalVetoedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, DAOProposalTimePeriodSetProcessor>();
        // DAO
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, DAOCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, FileInfosRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, FileInfosUploadedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilDisabledProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilEnabledProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SubsistStatusSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MetadataUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, MemberRemovedProcessor>();
        // election
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddressReplacedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateInfoUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, VotedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ElectionVotingEventRegisteredProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateElectedProcessor>();

        // governance
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSchemeAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSchemeThresholdRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceSchemeThresholdUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, GovernanceTokenSetProcessor>();
        // treasury
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TransferredProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryTransferredProcessor>();
        // vote
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, Vote.VoteSchemeCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, Vote.VotingItemRegisteredProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, Vote.VotedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, Vote.VoteWithdrawnProcessor>();
        
        //Network DAO
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, AssociationProposalCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ReferendumProposalCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ParliamentProposalCreatedProcessor>();
    }

    protected override string ClientId => "AElfIndexer_TomorrowDAO";
    protected override string Version => "5bb9074e8b0f45c0adca41d8bcd542ea";
}