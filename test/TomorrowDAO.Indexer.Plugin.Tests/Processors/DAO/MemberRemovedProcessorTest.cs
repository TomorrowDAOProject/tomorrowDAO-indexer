using Shouldly;
using TomorrowDAO.Indexer.Orleans.TestBase;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

[CollectionDefinition(ClusterCollection.Name)]
public class MemberRemovedProcessorTest : TomorrowDAOIndexerPluginTestBase
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
        var organizationIndex = await organizationIndexRepository.GetFromBlockStateSetAsync(
            IdGenerateHelper.GetId(ChainAelf, DAOId, address), ChainAelf);
        organizationIndex.ShouldBeNull();
    }
}