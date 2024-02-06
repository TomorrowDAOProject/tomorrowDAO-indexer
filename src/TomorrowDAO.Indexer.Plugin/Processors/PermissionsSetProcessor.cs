using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using PermissionInfo = TomorrowDAO.Indexer.Plugin.Entities.PermissionInfo;
using PermissionType = TomorrowDAO.Indexer.Plugin.Entities.PermissionType;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class PermissionsSetProcessor : DAOProcessorBase<PermissionsSet>
{
    public PermissionsSetProcessor(ILogger<DAOProcessorBase<PermissionsSet>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAORepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(PermissionsSet eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[PermissionsSet] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[PermissionsSet] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.PermissionAddress = eventValue.Here.ToBase58();
            DAOIndex.PermissionInfoList = eventValue.PermissionInfoList != null ?
                JsonConvert.SerializeObject(eventValue.PermissionInfoList?.PermissionInfos.Select(x => new PermissionInfo
                {
                    Where = x.Where.ToBase58(), Who = x.Who.ToBase58(), What = x.What, 
                    PermissionType = (PermissionType) Enum.Parse(typeof(PermissionType), x.PermissionType.ToString(), true)
                }).ToList())
                : string.Empty;
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[PermissionsSet] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[PermissionsSet] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}