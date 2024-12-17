using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.Referendum;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Referendum;

public class ReferendumReceiptCreatedProcessor : ReferendumProcessorBase<ReferendumReceiptCreated>
{
    public override async Task ProcessAsync(ReferendumReceiptCreated logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Referendum ReceiptCreated] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, logEvent.ProposalId?.ToHex());
        await SaveReceiptCreatedAsync(logEvent, context, NetworkDaoOrgType.Referendum);
    }
}