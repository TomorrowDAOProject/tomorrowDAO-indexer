using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.Contracts.Referendum;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Enums;
using TomorrowDAOIndexer.Processors.NetworkDao.Association;

namespace TomorrowDAOIndexer.Processors.NetworkDao.Parliament;

public class ParliamentReceiptCreatedProcessor : ParliamentProcessorBase<ReceiptCreated>
{
    public override async Task ProcessAsync(ReceiptCreated logEvent, LogEventContext context)
    {
        Logger.LogInformation("[Parliament ReceiptCreated] start. chainId={ChainId}, proposalId={ProposalId}",
            context.ChainId, logEvent.ProposalId?.ToHex());
        var referendumReceiptCreated = ObjectMapper.Map<ReceiptCreated, ReferendumReceiptCreated>(logEvent);
        referendumReceiptCreated.Symbol = string.Empty;
        referendumReceiptCreated.Amount = 1;
        await SaveReceiptCreatedAsync(referendumReceiptCreated, context, NetworkDaoOrgType.Parliament);
    }
}