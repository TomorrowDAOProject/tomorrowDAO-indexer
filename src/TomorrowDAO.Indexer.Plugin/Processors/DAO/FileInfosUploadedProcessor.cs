using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using FileInfoIndexer = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;
using FileInfoContract = TomorrowDAO.Contracts.DAO.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class FileInfosUploadedProcessor : DAOProcessorBase<FileInfosUploaded>
{
    public FileInfosUploadedProcessor(ILogger<AElfLogEventProcessorBase<FileInfosUploaded, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider)
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(FileInfosUploaded eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[FileInfosUploaded] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var uploadedFiles = eventValue.UploadedFiles;
            if (uploadedFiles == null)
            {
                Logger.LogInformation("[FileInfosUploaded] no files to upload: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }

            var DAOIndex = await DAOProvider.GetDaoAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[FileInfosUploaded] DAO not existed: Id={Id}, ChainId={ChainId}", 
                    DAOId, chainId);
                DAOIndex = ObjectMapper.Map<FileInfosUploaded, DAOIndex>(eventValue);
            }
            var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfoIndexer>>(DAOIndex.FileInfoList?? string.Empty) 
                                  ?? new List<FileInfoIndexer>();
            var currentFileIds = currentFileInfo.Select(x => x.File.Cid).ToList();
            var toAdd = ObjectMapper.Map<List<FileInfoContract>, List<FileInfoIndexer>>(uploadedFiles.Data.Values
                .ToList()).Where(x => !currentFileIds.Contains(x.File.Cid)).ToList();
            DAOIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo.Union(toAdd));
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[FileInfosUploaded] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[FileInfosUploaded] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}