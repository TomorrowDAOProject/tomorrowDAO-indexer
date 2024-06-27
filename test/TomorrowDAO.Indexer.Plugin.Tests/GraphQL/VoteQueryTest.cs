using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;
using Shouldly;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAO.Indexer.Plugin.GraphQL;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Xunit;

namespace TomorrowDAO.Indexer.Plugin.Tests.GraphQL;

public partial class QueryTest
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

    [Fact]
    public async Task GetVoteRecordCountAsyncTest()
    {
        await MockEventProcess(new Voted
        {
            VotingItemId = HashHelper.ComputeFrom("ss"),
            Voter = Address.FromBase58(User),
            Amount = 0,
            VoteTimestamp = DateTime.UtcNow.AddMinutes(1).ToTimestamp(),
            Option = VoteOption.Approved,
            VoteId = HashHelper.ComputeFrom(Id1),
            DaoId = HashHelper.ComputeFrom(Id1),
            VoteMechanism = VoteMechanism.UniqueVote,
            StartTime = DateTime.UtcNow.AddMinutes(1).ToTimestamp(),
            EndTime = DateTime.UtcNow.AddMinutes(200).ToTimestamp()
        }.ToLogEvent(), VoteVotedProcessor);

        var count = await Query.GetVoteRecordCountAsync(VoteRecordIndexRepository, ObjectMapper, new GetVoteRecordCountInput
        {
            ChainId = ChainAelf,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        count.ShouldBe(1);
    }
    
}