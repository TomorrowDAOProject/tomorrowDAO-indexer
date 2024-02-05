using AElfIndexer.Client;
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
    }

    protected override string ClientId => "AElfIndexer_tmrwdao";
    protected override string Version => "******";
}