using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.Association;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgMemberAddedProcessor : AssociationProcessorBase<MemberAdded>
{
    public override async Task ProcessAsync(MemberAdded logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Association MemberAdded] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgMemberAddedIndexAsync(logEvent, context, NetworkDaoOrgType.Association);
    }
}