using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public class MemberRemovedProcessor : DAOProcessorBase<MemberRemoved>
{
    public MemberRemovedProcessor(ILogger<AElfLogEventProcessorBase<MemberRemoved, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, IDAOProvider DAOProvider,
        IElectionProvider electionProvider)
        : base(logger, objectMapper, contractInfoOptions, DAOProvider, electionProvider)
    {
    }

    protected override async Task HandleEventAsync(MemberRemoved eventValue, LogEventContext context)
    {
        var DAOId = eventValue.DaoId?.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[MemberRemoved] START: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var removeMembers = eventValue.RemoveMembers;
            if (removeMembers == null)
            {
                Logger.LogInformation("[MemberRemoved] no member to remove: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
            }
            foreach (var member in removeMembers!.Value)
            {
                var address = member?.ToBase58();
                await DAOProvider.DeleteMemberAsync(chainId, IdGenerateHelper.GetId(chainId, DAOId, address));
            }
            Logger.LogInformation("[MemberRemoved] FINISH: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[MemberRemoved] Exception DAOId={DAOId}, ChainId={ChainId}", DAOId, chainId);
            throw;
        }
    }
}