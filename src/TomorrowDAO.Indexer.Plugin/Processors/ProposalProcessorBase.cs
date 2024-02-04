using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors;

public class ProposalProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;

    protected readonly IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo>
        ProposalRepository;

    protected ProposalProcessorBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<ProposalIndex, LogEventInfo> proposalRepository) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        ContractInfoOptions = contractInfoOptions.Value;
        ProposalRepository = proposalRepository;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].GovernanceContract;
    }
}