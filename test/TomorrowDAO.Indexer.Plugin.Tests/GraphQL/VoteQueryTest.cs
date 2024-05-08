using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using Moq;
using Shouldly;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public class VoteQueryTest : QueryTestBase
{
    private const string TransactionId = "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2";

    private static readonly string DaoId = HashHelper.ComputeFrom(Id1).ToHex();

    [Fact]
    public async Task GetVoterWithdrawnIndexAsync_Test()
    {
        
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var id = IdGenerateHelper.GetId(ChainAelf, DaoId, TransactionId);
        var voteSchemeIndex = await VoteWithdrawnRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        voteSchemeIndex.ShouldNotBeNull();

        var result = await Query.GetVoterWithdrawnIndexAsync(VoteWithdrawnRepository, ObjectMapper,
            new VoteWithdrawnIndexInput()
            {
                ChainId = ChainAelf,
                DaoId = DaoId,
                Voter = Address.FromBase58(User).ToBase58()
            });
        result.ShouldNotBeNull();
    }
}