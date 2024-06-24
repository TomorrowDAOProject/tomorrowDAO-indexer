using Shouldly;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.DAO;

public class MemberAddedProcessorTest : TomorrowDAOIndexerPluginTestBase
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
        var organizationIndex = await organizationIndexRepository.GetFromBlockStateSetAsync(
            IdGenerateHelper.GetId(ChainAelf, DAOId, address), ChainAelf);
        organizationIndex.ShouldNotBeNull();
        organizationIndex.Address.ShouldBe(address);
    }
}