using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAO.Indexer.Orleans.TestBase;

public class ClusterFixture : IDisposable, ISingletonDependency
{
    public ClusterFixture()
    {
        var builder = new TestClusterBuilder();
        var randomPort = DateTime.UtcNow.Second * 1000 + DateTime.UtcNow.Millisecond;
        builder.Options.BaseGatewayPort = 2000 + randomPort;
        builder.Options.BaseSiloPort = 1000 + randomPort;
        builder.Options.InitialSilosCount = 1;

        builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
        // builder.AddClientBuilderConfigurator<TestClientBuilderConfigurator>();
        Cluster = builder.Build();
        var retryCount = 30;
        while (true)
        {
            try
            {
                Cluster.Deploy();
                break;
            } 
            catch (Exception ex)
            {
                builder.Options.BaseGatewayPort++;
                builder.Options.BaseSiloPort++;
                Cluster = builder.Build();
                if (retryCount-- <= 0)
                {
                    throw;
                }
            }
        }
    }

    public void Dispose()
    {
        Cluster.StopAllSilos();
    }

    public TestCluster Cluster { get; private set; }

    private class TestSiloConfigurations : ISiloBuilderConfigurator
    {
        public void Configure(ISiloHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
                {
                    // services.AddSingleton<ITestGrain,TestGrain>();
                })
                .AddSimpleMessageStreamProvider(TomorrowDAOIndexerOrleansConsts.MessageStreamName)
                .AddMemoryGrainStorage("PubSubStore")
                .AddMemoryGrainStorageAsDefault();
        }
    }

    private class TestClientBuilderConfigurator : IClientBuilderConfigurator
    {
        public void Configure(IConfiguration configuration, IClientBuilder clientBuilder) => clientBuilder
            .AddSimpleMessageStreamProvider(TomorrowDAOIndexerOrleansConsts.MessageStreamName);
    }
}