using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using FileInfo = TomorrowDAO.Indexer.Plugin.Entities.FileInfo;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class FileInfosRemovedProcessor : DAOProcessorBase<FileInfosRemoved>
{
    public FileInfosRemovedProcessor(ILogger<AElfLogEventProcessorBase<FileInfosRemoved, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider)
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
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
            var DAOIndex = await DAOProvider.GetDaoAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[FileInfosRemoved] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            var removeFileInfo = eventValue.RemovedFiles;
            var currentFileInfo = JsonConvert.DeserializeObject<List<FileInfo>>(DAOIndex.FileInfoList?? string.Empty) 
                                  ?? new List<FileInfo>();
            if (removeFileInfo != null)
            {
                var removeFileIds = removeFileInfo.Data.Values.Select(x => x.File.Cid).ToList();
                DAOIndex.FileInfoList = JsonConvert.SerializeObject(currentFileInfo.Where(x => !removeFileIds.Contains(x.File.Cid)).ToList());
                await DAOProvider.SaveIndexAsync(DAOIndex, context);
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