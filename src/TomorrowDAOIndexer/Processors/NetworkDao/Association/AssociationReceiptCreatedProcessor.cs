using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.Referendum;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Association;

public class AssociationReceiptCreatedProcessor : AssociationProcessorBase<ReceiptCreated>
{
    public override async Task ProcessAsync(ReceiptCreated logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Association ReceiptCreated] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, logEvent.ProposalId?.ToHex());
        var referendumReceiptCreated = ObjectMapper.Map<ReceiptCreated, ReferendumReceiptCreated>(logEvent);
        referendumReceiptCreated.Symbol = string.Empty;
        referendumReceiptCreated.Amount = 1;
        await SaveReceiptCreatedAsync(referendumReceiptCreated, context, NetworkDaoOrgType.Association);
    }
}