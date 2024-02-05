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

public class HighCouncilMemberUpdatedProcessor : DaoProcessorBase<HighCouncilMemberUpdated>
{ 
    public HighCouncilMemberUpdatedProcessor(ILogger<DaoProcessorBase<HighCouncilMemberUpdated>> logger, IObjectMapper objectMapper, 
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> daoRepository,
        IAElfIndexerClientEntityRepository<ElectionIndex, LogEventInfo> electionRepository) : base(logger, objectMapper, contractInfoOptions, daoRepository, electionRepository)
    {
    }

    protected override async Task HandleEventAsync(HighCouncilMemberUpdated eventValue, LogEventContext context)
    {
        var daoId = eventValue.DaoId.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[HighCouncilMemberUpdated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(eventValue));
        try
        {
            var daoIndex = await DaoRepository.GetFromBlockStateSetAsync(daoId, chainId);
            if (daoIndex == null)
            {
                Logger.LogInformation("[HighCouncilMemberUpdated] dao not existed: Id={Id}, ChainId={ChainId}", daoId, chainId);
                return;
            }
            var updatedHighCouncilInfo = eventValue.UpdatedHighCouncilInfo;
            if (updatedHighCouncilInfo != null)
            {
                var termNumber = updatedHighCouncilInfo.TermNumber;
                daoIndex.TermNumber = termNumber;
                await SaveIndexAsync(daoIndex, context);
                foreach (var member in updatedHighCouncilInfo.MemberList.Data)
                {
                    await SaveIndexAsync(new ElectionIndex
                    {
                        Address = member.ToBase58(),
                        DaoId = daoId,
                        TermNumber = termNumber,
                        HighCouncilType = HighCouncilType.Member,
                        Id = IdGenerateHelper.GetId(chainId, daoId, member.ToBase58(), termNumber, HighCouncilType.Member) 
                    }, context);
                }
            }
            
            Logger.LogInformation("[HighCouncilMemberUpdated] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[HighCouncilMemberUpdated] Exception Id={daoId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}