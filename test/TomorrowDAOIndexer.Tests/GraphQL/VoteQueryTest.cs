using AElf;
using AElf.Types;
using Google.Protobuf.WellKnownTypes;
using Shouldly;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Input;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public class VoteQueryTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task GetVoterWithdrawnIndexAsync_Test()
    {
        await MockEventProcess(VoteWithdrawn(), VoteWithdrawnProcessor);
        
        var id = IdGenerateHelper.GetId(ChainId, DAOId, TransactionId);
        var voteWithdrawnIndex = await GetIndexById<VoteWithdrawnIndex>(id);
        voteWithdrawnIndex.ShouldNotBeNull();

        var result = await Query.GetVoterWithdrawnIndexAsync(VoteWithdrawnRepository, ObjectMapper,
            new VoteWithdrawnIndexInput
            {
                ChainId = ChainId,
                DaoId = DAOId,
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
        }, VoteVotedProcessor);

        var count = await Query.GetVoteRecordCountAsync(VoteRecordIndexRepository, ObjectMapper, new GetVoteRecordCountInput
        {
            ChainId = ChainId,
            // StartTime = "2024-05-28 00:00:00",
            // EndTime = "2024-05-28 23:59:59"
        });
        count.ShouldBe(1);
    }
}