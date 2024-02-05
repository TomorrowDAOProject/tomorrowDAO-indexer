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

public class FileInfosRemovedProcessor : DAOProcessorBase<FileInfosRemoved>
{
    public FileInfosRemovedProcessor(ILogger<DAOProcessorBase<FileInfosRemoved>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, DAORepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(FileInfosRemoved eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[FileInfosRemoved] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[FileInfosRemoved] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            var removeFileInfo = eventValue.RemovedFiles;
            var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfo>>(DAOIndex.FileInfoList);
            if (currentFileInfo != null)
            {
                var removeFileIds = removeFileInfo.FileInfos.Select(x => x.File.Hash).ToList();
                DAOIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo.Where(x => !removeFileIds.Contains(x.Hash)).ToList());
                await SaveIndexAsync(DAOIndex, context);
                Logger.LogInformation("[FileInfosRemoved] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[FileInfosRemoved] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}