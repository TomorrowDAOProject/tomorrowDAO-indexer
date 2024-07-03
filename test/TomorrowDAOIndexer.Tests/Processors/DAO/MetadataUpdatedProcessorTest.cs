using Shouldly;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class MetadataUpdatedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        await MockEventProcess(MetadataUpdated(), MetadataUpdatedProcessor);
        
        var DAOIndex = await GetIndexById<DAOIndex>(DAOId);
        DAOIndex.ShouldNotBeNull();
        DAOIndex.Name.ShouldBe("update");
        DAOIndex.LogoUrl.ShouldBe("update");
        DAOIndex.Description.ShouldBe("update");
        DAOIndex.SocialMedia.ShouldBe("{ \"update\": \"update\" }");
    }
}