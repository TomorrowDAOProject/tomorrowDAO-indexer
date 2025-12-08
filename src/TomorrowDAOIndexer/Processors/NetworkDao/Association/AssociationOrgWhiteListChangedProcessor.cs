using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgWhiteListChangedProcessor : AssociationProcessorBase<OrganizationWhiteListChanged>
{
    public override async Task ProcessAsync(OrganizationWhiteListChanged logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Association OrganizationWhiteListChanged] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgWhiteListChangedIndexAsync(logEvent, context, NetworkDaoOrgType.Association);
    }
}