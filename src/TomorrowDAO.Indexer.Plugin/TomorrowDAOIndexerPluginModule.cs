﻿using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.Processors;
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
        //add processors
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ProposalCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, ProposalReleasedProcessor>();
        // dao
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, DaoCreatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, FileInfosUploadedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, FileInfosRemovedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilDisabledProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilEnabledProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, HighCouncilMemberUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, PermissionsSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, SubsistStatusSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, TreasuryContractSetProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, VoteContractSetProcessor>();
        // election
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateAddressReplacedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateInfoUpdatedProcessor>();
        serviceCollection.AddSingleton<IAElfLogEventProcessor<LogEventInfo>, CandidateRemovedProcessor>();
        // treasury
    }

    protected override string ClientId => "AElfIndexer_tomorrowDAO";
    protected override string Version => "******";
}