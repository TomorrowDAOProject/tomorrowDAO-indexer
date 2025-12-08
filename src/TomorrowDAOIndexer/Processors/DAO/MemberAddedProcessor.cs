using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Plugin;
using TomorrowDAOIndexer.Entities;

namespace TomorrowDAOIndexer.Processors.DAO;

public class MemberAddedProcessor : DAOProcessorBase<MemberAdded>
{
    public override async Task ProcessAsync(MemberAdded logEvent, LogEventContext context)
    {
        var DAOId = logEvent.DaoId?.ToHex();
        var chainId = context.ChainId;
        Logger.LogInformation("[MemberAdded] START: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
        try
        {
            var addMembers = logEvent.AddMembers;
            if (addMembers == null || addMembers.Value.Count == 0)
            {
                Logger.LogInformation("[MemberAdded] no member to add: DAOId={Id}, ChainId={ChainId}", DAOId, chainId);
                return;
            }
            foreach (var member in addMembers!.Value)
            {
                var address = member?.ToBase58();
                await SaveEntityAsync(new OrganizationIndex
                {
                    Id = IdGenerateHelper.GetId(chainId, DAOId, address),
                    DAOId = DAOId, Address = address, CreateTime = context.Block.BlockTime
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