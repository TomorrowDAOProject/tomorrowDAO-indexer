using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Proposal;

public abstract class ProposalProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> ProposalRepository;
    protected readonly IGovernanceProvider GovernanceProvider;
    protected readonly IDAOProvider DAOProvider;

    protected ProposalProcessorBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> proposalRepository, 
        IGovernanceProvider governanceProvider, IDAOProvider DAOProvider) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        ContractInfoOptions = contractInfoOptions.Value;
        ProposalRepository = proposalRepository;
        GovernanceProvider = governanceProvider;
        this.DAOProvider = DAOProvider;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].GovernanceContract;
    }
    
    protected async Task SaveIndexAsync(ProposalIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await ProposalRepository.AddOrUpdateAsync(index);
    }

    protected async void UpdateVetoProposal(string proposalId, ProposalStatus proposalStatus,
        ProposalStage proposalStage, LogEventContext context)
    {
        UpdateVetoProposal(proposalId, proposalStatus, proposalStage, null, context);
    }

    protected async void UpdateVetoProposal(string proposalId, ProposalStatus proposalStatus, 
        ProposalStage proposalStage, DateTime? executeTime, LogEventContext context)
    {
        var proposalIndex = await ProposalRepository.GetFromBlockStateSetAsync(proposalId, context.ChainId);
        if (proposalIndex == null)
        {
            return;
        }
        ObjectMapper.Map(context, proposalIndex);
        proposalIndex.ProposalStatus = proposalStatus;
        proposalIndex.ProposalStage = proposalStage;
        await ProposalRepository.AddOrUpdateAsync(proposalIndex);
    }
}