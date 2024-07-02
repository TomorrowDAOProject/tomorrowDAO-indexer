using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class MemberAddedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MemberAdded(), MemberAddedProcessor);
        await CheckMemberExists(User);
        await CheckMemberExists(Creator);
        await CheckMemberExists(DAOCreator);
    }

    private async Task CheckMemberExists(string address)
    {
        var organizationIndex = await GetIndexById(IdGenerateHelper.GetId(ChainId, DAOId, address), organizationIndexRepository);
        organizationIndex.ShouldNotBeNull();
        organizationIndex.Address.ShouldBe(address);
        organizationIndex.BlockHeight.ShouldBe(BlockHeight);
    }
}