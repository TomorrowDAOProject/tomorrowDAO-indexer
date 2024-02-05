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

public class FileInfosUploadedProcessor : DaoProcessorBase<FileInfosUploaded>
{
    public FileInfosUploadedProcessor(ILogger<DaoProcessorBase<FileInfosUploaded>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(FileInfosUploaded eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[FileInfosUploaded] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var daoIndex = await DaoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                Logger.LogInformation("[FileInfosUploaded] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }

            var addFileInfo = eventValue.UploadedFiles;
            if (addFileInfo != null)
            {
                var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfo>>(daoIndex.FileInfoList);
                currentFileInfo.AddRange(addFileInfo.FileInfos.Select(x => new FileInfo
                {
                    Hash = x.File.Hash, Name = x.File.Name, Url = x.File.Url,
                }).ToList());
                daoIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo);
                await SaveIndexAsync(daoIndex, context);
                Logger.LogInformation("[FileInfosUploaded] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
            }
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[FileInfosUploaded] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}