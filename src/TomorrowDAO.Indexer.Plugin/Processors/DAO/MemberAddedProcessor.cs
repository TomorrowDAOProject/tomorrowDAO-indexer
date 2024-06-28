using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class MemberAddedProcessor : DAOProcessorBase<MemberAdded>
{
    public MemberAddedProcessor(ILogger<AElfLogEventProcessorBase<MemberAdded, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider)
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(MemberAdded eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId?.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[MemberAdded] START: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var addMembers = eventValue.AddMembers;
            if (addMembers == null || addMembers.Value.Count == 0)
            {
                Logger.LogInformation("[MemberAdded] no member to add: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            foreach (var member in addMembers!.Value)
            {
                var address = member?.ToBase58();
                await DAOProvider.SaveIndexAsync(new OrganizationIndex
                {
                    Id = IdGenerateHelper.GetId(chainId, DAOId, address),
                    DAOId = DAOId, Address = address, CreateTime = context.BlockTime
                }, context);
            }
            Logger.LogInformation("[MemberAdded] FINISH: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[MemberAdded] Exception DAOId={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}