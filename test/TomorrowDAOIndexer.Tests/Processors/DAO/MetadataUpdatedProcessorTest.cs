using Shouldly;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class MetadataUpdatedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(MetadataUpdated(), MetadataUpdatedProcessor);
        
        var DAOIndex = await GetIndexById(DAOId, DAOIndexRepository);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Name.ShouldBe("update");
        DAOIndex.LogoUrl.ShouldBe("update");
        DAOIndex.Description.ShouldBe("update");
        DAOIndex.SocialMedia.ShouldBe("{ \"update\": \"update\" }");
    }
}