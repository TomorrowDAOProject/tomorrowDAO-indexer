using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Proposal;

public class ProposalCreatedProcessor : ProposalProcessorBase<ProposalCreated>
{
    public ProposalCreatedProcessor(ILogger<AElfLogEventProcessorBase<ProposalCreated, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> proposalRepository,
        IGovernanceProvider governanceProvider) :
        base(logger, objectMapper, contractInfoOptions, proposalRepository, governanceProvider)
    {
    }

    protected override async Task HandleEventAsync(ProposalCreated eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId.ToHex();
        Logger.LogInformation("[ProposalCreated] start proposalId:{proposalId} chainId:{chainId} ", proposalId,
            chainId);
        var proposalIndex = await ProposalRepository.GetFromBlockStateSetAsync(proposalId, context.ChainId);
        if (proposalIndex != null)
        {
            Logger.LogInformation("[ProposalCreated] proposalIndex with id {id} chainId {chainId} has existed.",
                proposalId, chainId);
            return;
        }

        proposalIndex = ObjectMapper.Map<ProposalCreated, ProposalIndex>(eventValue);
        ObjectMapper.Map(context, proposalIndex);
        proposalIndex.TransactionInfo = OfCallTransactionInfo(eventValue.Transaction);
        proposalIndex.DeployTime = context.BlockTime;
        //Of governance info
        var governanceSubScheme =
            await GovernanceProvider.GetGovernanceSubSchemeAsync(chainId, proposalIndex.GovernanceSchemeId);
        if (governanceSubScheme != null)
        {
            ObjectMapper.Map(governanceSubScheme, proposalIndex);
        }

        proposalIndex.Id = proposalId;
        await ProposalRepository.AddOrUpdateAsync(proposalIndex);
        Logger.LogInformation("[ProposalCreated] end proposalId:{proposalId} chainId:{chainId} ", proposalId, chainId);
    }
    
    private static CallTransactionInfo OfCallTransactionInfo(ExecuteTransaction transaction)
    {
        //need decode
        var paramDict = new Dictionary<string, object>();

        var transactionInfo = new CallTransactionInfo
        {
            ToAddress = transaction.ToAddress?.ToBase58(),
            ContractMethodName = transaction.ContractMethodName,
            Params = paramDict
        };
        return transactionInfo;
    }
}