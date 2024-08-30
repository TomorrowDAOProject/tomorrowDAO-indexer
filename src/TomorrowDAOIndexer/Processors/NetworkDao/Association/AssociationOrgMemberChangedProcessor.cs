using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.Association;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgMemberChangedProcessor : AssociationProcessorBase<MemberChanged>
{
    public override async Task ProcessAsync(MemberChanged logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Association MemberChanged] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgMemberChangedIndexAsync(logEvent, context, NetworkDaoOrgType.Association);
    }
}