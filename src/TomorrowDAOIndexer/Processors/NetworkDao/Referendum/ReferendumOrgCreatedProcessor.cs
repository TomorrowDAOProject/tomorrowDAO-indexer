using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Referendum;

public class ReferendumOrgCreatedProcessor : ReferendumProcessorBase<OrganizationCreated>
{
    public override async Task ProcessAsync(OrganizationCreated logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Referendum OrganizationCreated] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgCreatedIndexAsync(logEvent, context, NetworkDaoOrgType.Referendum);
    }
}