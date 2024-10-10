using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Referendum;

public class ReferendumOrgThresholdChangedProcessor : ReferendumProcessorBase<OrganizationThresholdChanged>
{
    public override async Task ProcessAsync(OrganizationThresholdChanged logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Referendum OrganizationThresholdChanged] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgThresholdChangedIndexAsync(logEvent, context, NetworkDaoOrgType.Referendum);
    }
}