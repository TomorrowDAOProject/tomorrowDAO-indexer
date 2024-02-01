using AElfIndexer.Client;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAO.Indexer.Plugin.GraphQL;
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
    }

    protected override string ClientId => "AElfIndexer_tmrwdao";
    protected override string Version => "******";
}