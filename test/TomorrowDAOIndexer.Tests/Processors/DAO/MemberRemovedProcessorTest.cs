using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using Xunit;

namespace TomorrowDAOIndexer.Processors.DAO;

public class MemberRemovedProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(MemberAdded(), MemberAddedProcessor);
        await MockEventProcess(MemberRemoved(), MemberRemovedProcessor);
        await CheckMemberNotExists(User);
        await CheckMemberNotExists(Creator);
        await CheckMemberNotExists(DAOCreator);
    }

    private async Task CheckMemberNotExists(string address)
    {
        var organizationIndex = await GetIndexById(IdGenerateHelper.GetId(ChainId, DAOId, address), organizationIndexRepository);
        organizationIndex.ShouldBeNull();
    }
}