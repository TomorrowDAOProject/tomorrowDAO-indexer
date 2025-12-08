using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAOIndexer.Entities;
using FileInfoIndexer = TomorrowDAOIndexer.Entities.FileInfo;
using FileInfoContract = TomorrowDAO.Contracts.DAO.FileInfo;

namespace TomorrowDAOIndexer.Processors.DAO;

public class FileInfosUploadedProcessor : DAOProcessorBase<FileInfosUploaded>
{
    public override async Task ProcessAsync(FileInfosUploaded logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[FileInfosUploaded] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var uploadedFiles = logEvent.UploadedFiles;
            if (uploadedFiles == null)
            {
                Logger.LogInformation("[FileInfosUploaded] no files to upload: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }

            var DAOIndex = await GetEntityAsync<DAOIndex>(DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[FileInfosUploaded] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                DAOIndex = ObjectMapper.Map<FileInfosUploaded, DAOIndex>(logEvent);
            }
            var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfoIndexer>>(DAOIndex.FileInfoList?? string.Empty) 
                                  ?? new List<FileInfoIndexer>();
            var currentFileIds = currentFileInfo.Select(x => x.File.Cid).ToList();
            var toAdd = ObjectMapper.Map<List<FileInfoContract>, List<FileInfoIndexer>>(uploadedFiles.Data.Values
                .ToList()).Where(x => !currentFileIds.Contains(x.File.Cid)).ToList();
            DAOIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo.Union(toAdd));
            await SaveEntityAsync(DAOIndex, context);
            Logger.LogInformation("[FileInfosUploaded] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[FileInfosUploaded] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}