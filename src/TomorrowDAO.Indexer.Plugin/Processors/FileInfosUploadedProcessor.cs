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
using FileInfo = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class FileInfosUploadedProcessor : DAOProcessorBase<FileInfosUploaded>
{
    public FileInfosUploadedProcessor(ILogger<DAOProcessorBase<FileInfosUploaded>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAORepository, electionProvider)
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
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[FileInfosUploaded] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }

            var addFileInfo = eventValue.UploadedFiles;
            if (addFileInfo != null)
            {
                var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfo>>(DAOIndex.FileInfoList);
                currentFileInfo.AddRange(addFileInfo.Data.Values.Select(x => new FileInfo
                {
                    Cid = x.File.Cid, Name = x.File.Name, Url = x.File.Url,
                }).ToList());
                DAOIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo);
                await SaveIndexAsync(DAOIndex, context);
                Logger.LogInformation("[FileInfosUploaded] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[FileInfosUploaded] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}