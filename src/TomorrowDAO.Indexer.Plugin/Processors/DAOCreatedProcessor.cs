using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;
using FileInfo = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class DAOCreatedProcessor : DAOProcessorBase<DAOCreated>
{
    public DAOCreatedProcessor(ILogger<DAOProcessorBase<DAOCreated>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, DAORepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(DAOCreated eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[DAOCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex != null)
            {
                Logger.LogInformation("[DAOCreated] DAO already existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex = ObjectMapper.Map<DAOCreated, DAOIndex>(eventValue);
            DAOIndex.Id = DAOId;
            DAOIndex.GovernanceSchemeId = eventValue.GovernanceSchemeId?.ToHex();
            DAOIndex.Creator = eventValue.Creator?.ToBase58();
            DAOIndex.MetadataAdmin = eventValue.MetadataAdmin?.ToBase58();
            DAOIndex.FileInfoList = eventValue.FileInfoList != null ? 
                JsonConvert.SerializeObject(eventValue.FileInfoList.FileInfos.Select(x => new FileInfo
                {
                    Hash = x.File.Hash, Name = x.File.Name, Url = x.File.Url
                }).ToList()) 
                : string.Empty;
            DAOIndex.CreateTime = context.BlockTime;
            DAOIndex.SubsistStatus = true;
            DAOIndex.TreasuryContractAddress = ContractInfoOptions.ContractInfos[chainId].TreasuryContractAddress;
            DAOIndex.VoteContractAddress = ContractInfoOptions.ContractInfos[chainId].VoteContractAddress;
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[DAOCreated] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[DAOCreated] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}