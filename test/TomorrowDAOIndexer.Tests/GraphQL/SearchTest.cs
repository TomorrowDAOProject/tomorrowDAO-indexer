using Shouldly;
using Xunit;

namespace TomorrowDAOIndexer.GraphQL;

public class SearchTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task Test_1()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        
        var queryable = await DAOIndexRepository.GetQueryableAsync();
        queryable = QueryHelper.AddEqualFilter(queryable, "Metadata.ChainId", "AELF");
        // queryable = QueryHelper.AddEqualFilter(queryable, "Metadata.Block.BlockHeight", "100");
        var daoIndex = queryable.SingleOrDefault();
        daoIndex.ShouldNotBeNull();
    }
    
    [Fact]
    public async Task Test_2()
    {
        await MockEventProcess(MaxInfoDAOCreated(), DAOCreatedProcessor);
        
        var queryable = await DAOIndexRepository.GetQueryableAsync();
        queryable = queryable.Where(x => x.Metadata.ChainId == "AELF");
        // queryable = queryable.Where(x => x.Metadata.Block.BlockHeight == 100);
        var daoIndex = queryable.SingleOrDefault();
        daoIndex.ShouldNotBeNull();
    }
    
    // {
    //     "took": 2,
    //     "timed_out": false,
    //     "_shards": {
    //         "total": 1,
    //         "successful": 1,
    //         "skipped": 0,
    //         "failed": 0
    //     },
    //     "hits": {
    //         "total": {
    //             "value": 1,
    //             "relation": "eq"
    //         },
    //         "max_score": 1,
    //         "hits": [
    //         {
    //             "_index": "appid-version.daoindex",
    //             "_type": "_doc",
    //             "_id": "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
    //             "_score": 1,
    //             "_source": {
    //                 "id": "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",
    //                 "blockHeight": 100,
    //                 "creator": "2fbCtXNLVD2SC4AD6b8nqAkHtjqxRCfwvciX4MyH6257n8Gf63",
    //                 "name": "DAOName",
    //                 "logoUrl": "DAOLogoUrl",
    //                 "description": "DAODescription",
    //                 "socialMedia": "{ "name": "url" }",
    //                 "governanceToken": "ELF",
    //                 "isHighCouncilEnabled": false,
    //                 "maxHighCouncilMemberCount": 0,
    //                 "maxHighCouncilCandidateCount": 0,
    //                 "electionPeriod": 0,
    //                 "stakingAmount": 0,
    //                 "highCouncilTermNumber": 0,
    //                 "isTreasuryContractNeeded": false,
    //                 "subsistStatus": true,
    //                 "treasuryContractAddress": "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX",
    //                 "treasuryAccountAddress": "",
    //                 "isTreasuryPause": false,
    //                 "voteContractAddress": "vQfjcuW3RbGmkcL74YY4q3BX9UcH5rmwLmbQi3PsZxg8vE9Uk",
    //                 "electionContractAddress": "YeCqKprLBGbZZeRTkN1FaBLXsetY8QFotmVKqo98w9K6jK2PY",
    //                 "governanceContractAddress": "HJfhXPPL3Eb2wYPAc6ePmirenNzqGBAsynyeYF9tKSV2kHTAF",
    //                 "timelockContractAddress": "7VzrKvnFRjrK4duJz8HNA1nWf2AJcxwwGXzTtC4MC3tKUtdbH",
    //                 "activeTimePeriod": 7,
    //                 "vetoActiveTimePeriod": 3,
    //                 "pendingTimePeriod": 5,
    //                 "executeTimePeriod": 3,
    //                 "vetoExecuteTimePeriod": 1,
    //                 "createTime": "2024-07-03T10:05:54.6600570Z",
    //                 "isNetworkDAO": false,
    //                 "voterCount": 0,
    //                 "voteAmount": 0,
    //                 "withdrawAmount": 0,
    //                 "governanceMechanism": 2,
    //                 "metadata": {
    //                     "chainId": "AELF",
    //                     "block": {
    //                         "blockHash": "6b86b273ff34fce19d6b804eff5a3f5747ada4eaa22f1d49c01e52ddb7875b4b",
    //                         "blockHeight": 100,
    //                         "blockTime": "2024-07-03T10:05:53.9575980Z"
    //                     },
    //                     "isDeleted": false
    //                 }
    //             }
    //         }
    //         ]
    //     }
    // }
}