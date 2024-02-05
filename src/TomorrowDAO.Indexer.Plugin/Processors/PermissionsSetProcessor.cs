using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;
using PermissionInfo = TomorrowDAO.Indexer.Plugin.Entities.PermissionInfo;
using PermissionType = TomorrowDAO.Indexer.Plugin.Entities.PermissionType;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class PermissionsSetProcessor : DaoProcessorBase<PermissionsSet>
{
    public PermissionsSetProcessor(ILogger<DaoProcessorBase<PermissionsSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(PermissionsSet eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[PermissionsSet] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var daoIndex = await DaoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                Logger.LogInformation("[PermissionsSet] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex.PermissionAddress = eventValue.Here.ToBase58();
            daoIndex.PermissionInfoList = eventValue.PermissionInfoList != null ?
                JsonConvert.SerializeObject(eventValue.PermissionInfoList?.PermissionInfos.Select(x => new PermissionInfo
                {
                    Where = x.Where.ToBase58(), Who = x.Who.ToBase58(), What = x.What, 
                    PermissionType = (PermissionType) Enum.Parse(typeof(PermissionType), x.PermissionType.ToString(), true)
                }).ToList())
                : string.Empty;
            await SaveIndexAsync(daoIndex, context);
            Logger.LogInformation("[PermissionsSet] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[PermissionsSet] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}