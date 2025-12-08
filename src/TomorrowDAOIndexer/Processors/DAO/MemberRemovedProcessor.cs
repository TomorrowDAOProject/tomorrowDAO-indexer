using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.DAO;

public class MemberRemovedProcessor : DAOProcessorBase<MemberRemoved>
{
    public override async Task ProcessAsync(MemberRemoved logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId?.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[MemberRemoved] START: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var removeMembers = logEvent.RemoveMembers;
            if (removeMembers == null)
            {
                Logger.LogInformation("[MemberRemoved] no member to remove: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
            }
            foreach (var member in removeMembers!.Value)
            {
                var address = member?.ToBase58();
                await DeleteEntityAsyncAndCheck<OrganizationIndex>(IdGenerateHelper.GetId(chainId, DAOId, address));
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