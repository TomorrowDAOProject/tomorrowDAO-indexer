using AElf;
using Shouldly;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.Processors.Vote;

public class VoteWithdrawnProcessorTest : TomorrowDAOIndexerPluginTestBase
{
    private const string TransactionId = "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2";
    
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var id = IdGenerateHelper.GetId(ChainAelf, HashHelper.ComputeFrom(Id1).ToHex(), TransactionId);
        var voteSchemeIndex = await VoteWithdrawnRepository.GetFromBlockStateSetAsync(id, ChainAelf);
        voteSchemeIndex.ShouldNotBeNull();
        voteSchemeIndex.WithdrawAmount.ShouldBe(10);
    }
}