using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using TomorrowDAOIndexer.Enums;
using ProposalCreated = AElf.Standards.ACS3.ProposalCreated;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Referendum;

public class ReferendumProposalCreatedProcessor : ReferendumProcessorBase<ProposalCreated>
{
    public override async Task ProcessAsync(ProposalCreated eventValue, LogEventContext context)
    {
        Logger.LogInformation("[Referendum ProposalCreated] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, eventValue.ProposalId?.ToHex());
        await SaveProposalIndexAsync(eventValue, context, NetworkDaoOrgType.Referendum);
    }
}