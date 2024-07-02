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
        context.Services.AddTransient<ILogEventProcessor, FileInfosRemovedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, FileInfosUploadedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, HighCouncilDisabledProcessor>();
        context.Services.AddTransient<ILogEventProcessor, HighCouncilEnabledProcessor>();
        context.Services.AddTransient<ILogEventProcessor, SubsistStatusSetProcessor>();
        context.Services.AddTransient<ILogEventProcessor, MetadataUpdatedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, MemberAddedProcessor>();
        context.Services.AddTransient<ILogEventProcessor, MemberRemovedProcessor>();
    }
}