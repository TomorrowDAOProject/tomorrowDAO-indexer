using Shouldly;
using TomorrowDAOIndexer.GraphQL;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class DAOCreatedProcessorTest: TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task Test()
    {
        await MockEventProcess(MaxInfoDAOCreated(), _DAOCreatedProcessor);
        
        var entities = await Query.DAOIndex(_repository, _objectMapper, new GetDaoListInput { ChainId = ChainId });
        entities.Count.ShouldBe(1);
    }
}