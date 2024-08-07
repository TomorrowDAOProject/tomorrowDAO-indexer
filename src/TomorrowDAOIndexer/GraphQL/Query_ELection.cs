using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getElectionHighCouncilConfig")]
    public static async Task<ElectionHighCouncilConfigDto> GetElectionHighCouncilConfigAsync(
        [FromServices] IReadOnlyRepository<ElectionHighCouncilConfigIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetElectionHighCouncilListInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DaoId == input.DaoId);
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        var result = queryable.ToList();

        return new ElectionHighCouncilConfigDto
        {
            TotalCount = count,
            Items = objectMapper.Map<List<ElectionHighCouncilConfigIndex>, List<ElectionHighCouncilConfig>>(result)
        };
    }
    
    [Name("getElectionVotingItem")]
    public static async Task<ElectionVotingItemDto> GetElectionVotingItemIndexAsync(
        [FromServices] IReadOnlyRepository<ElectionVotingItemIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetElectionVotingItemIndexInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DaoId == input.DaoId);
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        var result = queryable.ToList();

        return new ElectionVotingItemDto
        {
            TotalCount = count,
            Items = objectMapper.Map<List<ElectionVotingItemIndex>, List<ElectionVotingItem>>(result)
        };
    }
    
    [Name("getElectionCandidateElected")]
    public static async Task<ElectionCandidateElectedDto> GetElectionCandidateElectedAsync(
        [FromServices] IReadOnlyRepository<ElectionCandidateElectedIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetElectionCandidateElectedInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DaoId == input.DaoId);
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        var result = queryable.ToList();

        return new ElectionCandidateElectedDto
        {
            TotalCount = count,
            Items = objectMapper.Map<List<ElectionCandidateElectedIndex>, List<ElectionCandidateElected>>(result)
        };
    }


    [Name("getElectionListAsync")]
    public static async Task<List<ElectionDto>> GetElectionListAsync(
        [FromServices] IReadOnlyRepository<ElectionIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetChainBlockHeightInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (input.StartBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight >= input.StartBlockHeight);
        }
        if (input.EndBlockHeight > 0)
        {
            queryable = queryable.Where(a => a.BlockHeight <= input.EndBlockHeight);
        }
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
            .Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderBy(a => a.BlockHeight);
        var result = queryable.ToList();
        
        return objectMapper.Map<List<ElectionIndex>, List<ElectionDto>>(result);
    }

    [Name("getHighCouncilListAsync")]
    public static async Task<ElectionListPageResultDto> GetHighCouncilListAsync(
        [FromServices] IReadOnlyRepository<ElectionIndex> repository,
        [FromServices] IObjectMapper objectMapper, GetHighCouncilListInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        var stringHighCouncilType = input.HighCouncilType;
        queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId)
                .Where(a => a.TermNumber == input.TermNumber);
        if (!string.IsNullOrEmpty(stringHighCouncilType))
        {
            queryable = stringHighCouncilType switch
            {
                "Member" => queryable.Where(a => a.HighCouncilType == HighCouncilType.Member),
                "Candidate" => queryable.Where(a => a.HighCouncilType == HighCouncilType.Candidate),
                "BlackList" => queryable.Where(a => a.HighCouncilType == HighCouncilType.BlackList),
                _ => queryable
            };
            queryable = queryable.Where(a => a.DAOId == input.DAOId);
        }
        if (!string.IsNullOrEmpty(input.DAOId))
        {
            queryable = queryable.Where(a => a.DAOId == input.DAOId);
        }
        var count = queryable.Count();
        queryable = queryable.Skip(input.SkipCount).Take(input.MaxResultCount)
            .OrderByDescending(a => a.StakeAmount);
        return new ElectionListPageResultDto
        {
            TotalCount = count,
            DataList = objectMapper.Map<List<ElectionIndex>, List<ElectionDto>>(queryable.ToList())
        };
    }
}