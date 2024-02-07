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

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class DAOCreatedProcessor : DAOProcessorBase<DAOCreated>
{
    public DAOCreatedProcessor(ILogger<DAOProcessorBase<DAOCreated>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IElectionProvider electionProvider) 
        : base(logger, objectMapper, contractInfoOptions, DAORepository, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(DAOCreated eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[DAOCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex != null)
            {
                Logger.LogInformation("[DAOCreated] DAO already existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            DAOIndex = ObjectMapper.Map<DAOCreated, DAOIndex>(eventValue);
            DAOIndex.CreateTime = context.BlockTime;
            DAOIndex.SubsistStatus = true;
            DAOIndex.TreasuryContractAddress = ContractInfoOptions.ContractInfos[chainId].TreasuryContractAddress;
            DAOIndex.VoteContractAddress = ContractInfoOptions.ContractInfos[chainId].VoteContractAddress;
            await SaveIndexAsync(DAOIndex, context);
            Logger.LogInformation("[DAOCreated] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[DAOCreated] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}