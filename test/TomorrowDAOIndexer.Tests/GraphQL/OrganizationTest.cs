using Shouldly;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public partial class QueryTest
{
    [Fact]
    public async Task GetMemberListAsync_Test()
    {
        await MockEventProcess(MemberAdded(), MemberAddedProcessor);
    
        var result = await Query.GetMemberListAsync(organizationIndexRepository, ObjectMapper, new GetMemberListInput
        {
            ChainId = ChainId, DAOId = DAOId, SkipCount = 0, MaxResultCount = 10
        });
        result.TotalCount.ShouldBe(2);
        var list = result.Data;
        list.Count.ShouldBe(2);
        var addresses = string.Join(",", list.Select(x => x.Address));
        addresses.ShouldContain(User);
        addresses.ShouldContain(DAOCreator);
    }
    
    [Fact]
    public async Task GetMemberAsync_Test()
    {
        await MockEventProcess(MemberAdded(), MemberAddedProcessor);

        var member= await Query.GetMemberAsync(organizationIndexRepository, ObjectMapper , new GetMemberInput
        {
            ChainId = ChainId, DAOId = DAOId, Address = User
        });
        member.ShouldNotBeNull();
        member.Address.ShouldBe(User);
    }
}