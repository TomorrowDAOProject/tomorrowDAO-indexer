using AeFinder.App.TestBase;
using Microsoft.Extensions.DependencyInjection;
using TomorrowDAOIndexer.Processors;
using TomorrowDAOIndexer.Processors.DAO;
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
    }
}