using Shouldly;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;
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
        var organizationIndex = await GetIndexById<OrganizationIndex>(IdGenerateHelper.GetId(ChainId, DAOId, address));
        organizationIndex.ShouldNotBeNull();
        organizationIndex.Address.ShouldBe(address);
        organizationIndex.BlockHeight.ShouldBe(BlockHeight);
    }
}