using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.DAO;

public abstract class DAOProcessorBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>,new()
{
    protected readonly ILogger<DAOProcessorBase<TEvent>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly ContractInfoOptions ContractInfoOptions;
    protected readonly IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository;
    protected readonly IElectionProvider ElectionProvider;
    
    protected DAOProcessorBase(ILogger<DAOProcessorBase<TEvent>> logger,
        IObjectMapper objectMapper, IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IAElfIndexerClientEntityRepository<DAOIndex, LogEventInfo> DAORepository, 
        IElectionProvider electionProvider) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        this.DAORepository = DAORepository;
        ElectionProvider = electionProvider;
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].DAOContractAddress;
    }

    protected async Task SaveIndexAsync(DAOIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await DAORepository.AddOrUpdateAsync(index);
    }
}