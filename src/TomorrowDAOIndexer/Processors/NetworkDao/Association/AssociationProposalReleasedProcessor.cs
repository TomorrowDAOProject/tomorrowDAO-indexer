using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationProposalReleasedProcessor  : AssociationProcessorBase<ProposalReleased>
{
    public override async Task ProcessAsync(ProposalReleased logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Association ProposalReleased] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, logEvent.ProposalId?.ToHex());
        await SaveProposalReleasedIndexAsync(logEvent, context, NetworkDaoOrgType.Association);
    }
}