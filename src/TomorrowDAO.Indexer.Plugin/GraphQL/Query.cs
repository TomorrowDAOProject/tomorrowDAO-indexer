using System.Globalization;
using AElfIndexer.Client;
using AElfIndexer.Client.Providers;
using AElfIndexer.Grains;
using AElfIndexer.Grains.Grain.Client;
using AElfIndexer.Grains.State.Client;
using GraphQL;
using Nest;
using Orleans;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.GraphQL.Dto;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.GraphQL;

public partial class Query
{
    [Name("syncState")]
    public static async Task<SyncStateDto> SyncStateAsync(
        [FromServices] IClusterClient clusterClient,
        [FromServices] IAElfIndexerClientInfoProvider clientInfoProvider,
        GetSyncStateDto input)
    {
        var version = clientInfoProvider.GetVersion();
        var clientId = clientInfoProvider.GetClientId();
        var blockStateSetInfoGrain =
            clusterClient.GetGrain<IBlockStateSetInfoGrain>(
                GrainIdHelper.GenerateGrainId("BlockStateSetInfo", clientId, input.ChainId, version));
        var confirmedHeight = await blockStateSetInfoGrain.GetConfirmedBlockHeight(input.FilterType);
        return new SyncStateDto
        {
            ConfirmedBlockHeight = confirmedHeight
        };
    }
    
    [Name("getDAOList")]
    public static async Task<List<DAOInfoDto>> GetDAOListAsync(
        [FromServices] IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetChainBlockHeightInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<DAOIndex>, QueryContainer>>();
        mustQuery.Add(q => q.Term(i
            => i.Field(f => f.ChainId).Value(input.ChainId)));
    
        if (input.StartBlockHeight > 0)
        {
            mustQuery.Add(q => q.Range(i
                => i.Field(f => f.BlockHeight).GreaterThanOrEquals(input.StartBlockHeight)));
        }
    
        if (input.EndBlockHeight > 0)
        {
            mustQuery.Add(q => q.Range(i
                => i.Field(f => f.BlockHeight).LessThanOrEquals(input.EndBlockHeight)));
        }
    
        QueryContainer Filter(QueryContainerDescriptor<DAOIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetSortListAsync(Filter, 
            sortFunc: s => s.Ascending(a => a.BlockHeight),
            skip: input.SkipCount, limit: input.MaxResultCount);
        return objectMapper.Map<List<DAOIndex>, List<DAOInfoDto>>(result.Item2);
    }
    
    [Name("getDaoCount")]
    public static async Task<long> GetDaoCountAsync(
        [FromServices] IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetDaoCountInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<DAOIndex>, QueryContainer>>();

        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }
        
        if (!input.StartTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.StartTime, DateFormat, CultureInfo.InvariantCulture);
            mustQuery.Add(q => q.DateRange(i => i.Field(f => f.CreateTime).GreaterThanOrEquals(dateTime)));
        }

        if (!input.EndTime.IsNullOrWhiteSpace())
        {
            var dateTime = DateTime.ParseExact(input.EndTime, DateFormat, CultureInfo.InvariantCulture);
            mustQuery.Add(q => q.DateRange(i => i.Field(f => f.CreateTime).LessThanOrEquals(dateTime)));
        }

        QueryContainer Filter(QueryContainerDescriptor<DAOIndex> f) => f.Bool(b => b.Must(mustQuery));

        var result = await repository.CountAsync(Filter);
        return result?.Count ?? 0;
    }
    
    [Name("getDAOVoterRecord")]
    public static async Task<List<DaoVoterRecordIndexDto>> GetDAOVoterRecordAsync(
        [FromServices] IAElfIndexerClientEntityRepository<DaoVoterRecordIndex, LogEventInfo> repository,
        [FromServices] IObjectMapper objectMapper,
        GetDAOVoterRecordInput input)
    {
        var mustQuery = new List<Func<QueryContainerDescriptor<DaoVoterRecordIndex>, QueryContainer>>();

        if (!input.ChainId.IsNullOrWhiteSpace())
        {
            mustQuery.Add(q => q.Term(i
                => i.Field(f => f.ChainId).Value(input.ChainId)));
        }
    
        if (!input.DaoIds.IsNullOrEmpty())
        {
            mustQuery.Add(q => q.Terms(i
                => i.Field(f => f.DaoId).Terms(input.DaoIds)));
        }
    
        if (input.VoterAddressList.IsNullOrEmpty())
        {
            mustQuery.Add(q => q.Terms(i
                => i.Field(f => f.VoterAddress).Terms(input.VoterAddressList)));
        }
    
        QueryContainer Filter(QueryContainerDescriptor<DaoVoterRecordIndex> f) =>
            f.Bool(b => b.Must(mustQuery));
    
        var result = await repository.GetSortListAsync(Filter);
        return objectMapper.Map<List<DaoVoterRecordIndex>, List<DaoVoterRecordIndexDto>>(result.Item2);
    }
}