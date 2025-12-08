using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationOrgCreatedProcessor : AssociationProcessorBase<OrganizationCreated>
{
    public override async Task ProcessAsync(OrganizationCreated logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Association OrganizationCreated] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgCreatedIndexAsync(logEvent, context, NetworkDaoOrgType.Association);
    }
}