using AeFinder.Sdk.Processor;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAOIndexer.GraphQL;
using TomorrowDAOIndexer.Processors.DAO;
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
    }
}