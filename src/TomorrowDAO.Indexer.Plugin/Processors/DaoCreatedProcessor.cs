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

public class DaoCreatedProcessor : DaoProcessorBase<DAOCreated>
{
    public DaoCreatedProcessor(ILogger<DaoProcessorBase<DAOCreated>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(DAOCreated eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        _logger.LogInformation("[DAOCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var daoIndex = await _daoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex != null)
            {
                _logger.LogInformation("[DAOCreated] dao already existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            daoIndex = _objectMapper.Map<DAOCreated, DaoIndex>(eventValue);
            daoIndex.Id = daoId;
            daoIndex.GovernanceSchemeId = eventValue.GovernanceSchemeId?.ToHex();
            daoIndex.Creator = eventValue.Creator?.ToBase58();
            daoIndex.MetadataAdmin = eventValue.MetadataAdmin?.ToBase58();
            daoIndex.FileInfoList = eventValue.FileInfoList != null ? 
                JsonConvert.SerializeObject(eventValue.FileInfoList.FileInfos.Select(x => new FileInfo
                {
                    Hash = x.File.Hash, Name = x.File.Name, Url = x.File.Url
                }).ToList()) 
                : string.Empty;
            daoIndex.CreateTime = context.BlockTime;
            daoIndex.SubsistStatus = true;
            daoIndex.TreasuryContractAddress = _contractInfoOptions.ContractInfos[chainId].TreasuryContractAddress;
            daoIndex.VoteContractAddress = _contractInfoOptions.ContractInfos[chainId].VoteContractAddress;
            await SaveIndexAsync(daoIndex, context);
            _logger.LogInformation("[DAOCreated] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[DAOCreated] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}