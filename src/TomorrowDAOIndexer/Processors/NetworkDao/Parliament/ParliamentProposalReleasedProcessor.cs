using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.NetworkDao.Association;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

public class ParliamentProposalReleasedProcessor  : ParliamentProcessorBase<ProposalReleased>
{
    public override async Task ProcessAsync(ProposalReleased logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Parliament ProposalReleased] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, logEvent.ProposalId?.ToHex());
        await SaveProposalReleasedIndexAsync(logEvent, context, NetworkDaoOrgType.Parliament);
    }
}