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

public class FileInfosRemovedProcessor : DaoProcessorBase<FileInfosRemoved>
{
    public FileInfosRemovedProcessor(ILogger<DaoProcessorBase<FileInfosRemoved>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(FileInfosRemoved eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        _logger.LogInformation("[FileInfosRemoved] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var daoIndex = await _daoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                _logger.LogInformation("[FileInfosRemoved] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            var removeFileInfo = eventValue.RemovedFiles;
            var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfo>>(daoIndex.FileInfoList);
            if (currentFileInfo != null)
            {
                var removeFileIds = removeFileInfo.FileInfos.Select(x => x.File.Hash).ToList();
                daoIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo.Where(x => !removeFileIds.Contains(x.Hash)).ToList());
                await SaveIndexAsync(daoIndex, context);
                _logger.LogInformation("[FileInfosRemoved] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[FileInfosRemoved] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}