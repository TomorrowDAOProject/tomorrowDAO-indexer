using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class HighCouncilMemberUpdatedProcessor : DAOProcessorBase<HighCouncilMemberUpdated>
{ 
    public HighCouncilMemberUpdatedProcessor(ILogger<DAOProcessorBase<HighCouncilMemberUpdated>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, DAORepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilMemberUpdated eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilMemberUpdated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            DAOId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var DAOIndex = await DAORepository.GetFromBlockStateSetAsync(DAOId, chainId);
            if (DAOIndex == null)
            {
                Logger.LogInformation("[HighCouncilMemberUpdated] DAO not existed: Id={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            var updatedHighCouncilInfo = eventValue.UpdatedHighCouncilInfo;
            if (updatedHighCouncilInfo != null)
            {
                var termNumber = updatedHighCouncilInfo.TermNumber;
                DAOIndex.TermNumber = termNumber;
                await SaveIndexAsync(DAOIndex, context);
                foreach (var member in updatedHighCouncilInfo.MemberList.Data)
                {
                    await SaveIndexAsync(new ElectionIndex
                    {
                        Address = member.ToBase58(),
                        DaoId = DAOId,
                        TermNumber = termNumber,
                        HighCouncilType = HighCouncilType.Member,
                        Id = IdGenerateHelper.GetId(chainId, DAOId, member.ToBase58(), termNumber, HighCouncilType.Member) 
                    }, context);
                }
            }
            
            Logger.LogInformation("[HighCouncilMemberUpdated] FINISH: Id={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilMemberUpdated] Exception Id={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}