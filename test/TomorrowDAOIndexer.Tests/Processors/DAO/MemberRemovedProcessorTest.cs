using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
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
        var organizationIndex = await GetIndexById<OrganizationIndex>(IdGenerateHelper.GetId(ChainId, DAOId, address));
        organizationIndex.ShouldBeNull();
    }
}