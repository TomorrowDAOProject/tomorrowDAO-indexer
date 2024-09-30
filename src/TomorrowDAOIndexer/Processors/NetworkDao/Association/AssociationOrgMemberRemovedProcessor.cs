using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.Association;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgMemberRemovedProcessor : AssociationProcessorBase<MemberRemoved>
{
    public override async Task ProcessAsync(MemberRemoved logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Association MemberRemoved] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgMemberRemovedIndexAsync(logEvent, context, NetworkDaoOrgType.Association);
    }
}