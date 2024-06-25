using Shouldly;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class OrganizationQueryTest : QueryTestBase
{
    [Fact]
    public async Task GetMemberListAsync_Test()
    {
        await MockEventProcess(MemberAdded(), MemberAddedProcessor);

        var result = await Query.GetMemberListAsync(organizationIndexRepository, ObjectMapper, new GetMemberListInput
        {
            ChainId = ChainAelf, DAOId = DAOId, SkipCount = 0, MaxResultCount = 10
        });
        result.TotalCount.ShouldBe(2);
        var list = result.Data;
        list.Count.ShouldBe(2);
        var addresses = string.Join(",", list.Select(x => x.Address));
        addresses.ShouldContain(User);
        addresses.ShouldContain(DAOCreator);
    }

    [Fact]
    public async Task GetIsMemberAsync_Test()
    {
        await MockEventProcess(MemberAdded(), MemberAddedProcessor);

        var isMember= await Query.GetIsMemberAsync(organizationIndexRepository, new GetIsMemberInput
        {
            ChainId = ChainAelf, DAOId = DAOId, Address = User
        });
        isMember.ShouldBe(true);
    }
}