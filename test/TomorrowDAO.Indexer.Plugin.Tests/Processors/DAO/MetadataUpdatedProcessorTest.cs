using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

[CollectionDefinition(ClusterCollection.Name)]
public class MetadataUpdatedProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(MetadataUpdated(), MetadataUpdatedProcessor);
        
        var DAOIndex = await DAOIndexRepository.GetFromBlockStateSetAsync(DAOId, ChainAelf);
        DAOIndex.ShouldNotBeNull();
        var metadata = DAOIndex.Metadata;
        metadata.ShouldNotBeNull();
        metadata.Name.ShouldBe("update");
        metadata.LogoUrl.ShouldBe("update");
        metadata.Description.ShouldBe("update");
        metadata.SocialMedia.ShouldBe("{ \"update\": \"update\" }");
    }
}