using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.NetworkDao.Association;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Referendum;

public class ReferendumProposalReleasedProcessor  : ReferendumProcessorBase<ProposalReleased>
{
    public override async Task ProcessAsync(ProposalReleased logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Referendum ProposalReleased] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, logEvent.ProposalId?.ToHex());
        await SaveProposalReleasedIndexAsync(logEvent, context, NetworkDaoOrgType.Referendum);
    }
}