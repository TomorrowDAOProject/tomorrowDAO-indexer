using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.NetworkDao.Association;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

public class ParliamentOrgWhiteListChangedProcessor : ParliamentProcessorBase<OrganizationWhiteListChanged>
{
    public override async Task ProcessAsync(OrganizationWhiteListChanged logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Parliament OrganizationWhiteListChanged] start. chainId={0}, organization={1}",
            context.ChainId, logEvent.OrganizationAddress?.ToBase58());
        await SaveOrgWhiteListChangedIndexAsync(logEvent, context, NetworkDaoOrgType.Parliament);
    }
}