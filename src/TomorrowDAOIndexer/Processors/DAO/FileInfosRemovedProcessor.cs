using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;
using FileInfo = TomorrowDAOIndexer.Entities.FileInfo;

namespace TomorrowDAOIndexer.Processors.DAO;

public class FileInfosRemovedProcessor : DAOProcessorBase<FileInfosRemoved>
{
    public override async Task ProcessAsync(FileInfosRemoved logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[FileInfosRemoved] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[FileInfosRemoved] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            var removeFileInfo = logEvent.RemovedFiles;
            var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfo>>(DAOIndex.FileInfoList?? string.Empty) 
                                  ?? new List<FileInfo>();
            if (removeFileInfo != null)
            {
                var removeFileIds = removeFileInfo.Data.Values.Select(x => x.File.Cid).ToList();
                DAOIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo.Where(x => !removeFileIds.Contains(x.File.Cid)).ToList());
                await SaveEntityAsync(DAOIndex, context);
                Logger.LogInformation("[FileInfosRemoved] REMOVED: Id={Id}, ChainId={ChainId}", DAOId, chainId);
            }
            Logger.LogInformation("[FileInfosRemoved] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[FileInfosRemoved] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}