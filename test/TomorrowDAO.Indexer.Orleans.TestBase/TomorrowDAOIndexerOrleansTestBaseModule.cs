using Microsoft.Extensions.DependencyInjection;
using Orleans;
using TomorrowDAO.Indexer.TestBase;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace TomorrowDAO.Indexer.Orleans.TestBase;

[DependsOn(typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule),
    typeof(TomorrowDAOIndexerTestBaseModule)
)]
public class TomorrowDAOIndexerOrleansTestBaseModule : AbpModule
{
    private static readonly object Lock = new object();
    private ClusterFixture _fixture;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        if (_fixture == null)
        {
            lock(Lock)
            {
                if (_fixture == null)
                {
                    _fixture = new ClusterFixture();   
                }
            }
        }

        context.Services.AddSingleton<ClusterFixture>(_fixture);
        context.Services.AddSingleton<IClusterClient>(sp => _fixture.Cluster.Client);
    }
}