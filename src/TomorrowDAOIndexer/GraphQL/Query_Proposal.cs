using System.Globalization;
using AeFinder.Sdk;
using GraphQL;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.GraphQL.Dto;
using TomorrowDAOIndexer.GraphQL.Input;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.GraphQL;

public partial class Query
{
    [Name("getSyncProposalInfos")]
    public static async Task<List<ProposalSyncDto>> GetSyncProposalInfosAsync(
        [FromServices] IReadOnlyRepository<ProposalIndex> repository,
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
        return objectMapper.Map<List<ProposalIndex>, List<ProposalSyncDto>>(result);
    }
    
    [Name("getProposalCount")]
    public static async Task<ProposalCountDto> GetProposalCountAsync(
        [FromServices] IReadOnlyRepository<ProposalIndex> repository,
        GetProposalCountInput input)
    {
        var queryable = await repository.GetQueryableAsync();
        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.Metadata.ChainId == input.ChainId);
        }
        if (!input.DaoId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.DAOId == input.DaoId);
        }
        // if (!input.StartTime.IsNullOrWhiteSpace())
        // {
        //     var dateTime = DateTime.ParseExact(input.StartTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
        //     queryable = queryable.Where(a => a.DeployTime >= dateTime);
        // }
        // if (!input.EndTime.IsNullOrWhiteSpace())
        // {
        //     var dateTime = DateTime.ParseExact(input.EndTime, TomorrowDAOConst.DateFormat, CultureInfo.InvariantCulture);
        //     queryable = queryable.Where(a => a.DeployTime <= dateTime);
        // }

        return new ProposalCountDto
        {
            Count = queryable.Count()
        };
    }
}