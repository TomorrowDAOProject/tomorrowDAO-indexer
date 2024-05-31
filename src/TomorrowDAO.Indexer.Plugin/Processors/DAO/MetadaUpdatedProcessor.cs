using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;
using MetadataContract = TomorrowDAO.Contracts.DAO.Metadata;
using MetadataIndexer = TomorrowDAO.Indexer.Plugin.Entities.Metadata;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class MetadataUpdatedProcessor : DAOProcessorBase<MetadataUpdated>
{
    public MetadataUpdatedProcessor(ILogger<AElfLogEventProcessorBase<MetadataUpdated, LogEventInfo>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider)
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(MetadataUpdated eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[MetadataUpdated] START: DAOId={DAOId}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAOProvider.GetDAOAsync(chainId, DAOId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[MetadataUpdated] DAO not already existed: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex.Metadata = ObjectMapper.Map<MetadataContract, MetadataIndexer>(eventValue.Metadata);
            await DAOProvider.SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[MetadataUpdated] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[MetadataUpdated] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}