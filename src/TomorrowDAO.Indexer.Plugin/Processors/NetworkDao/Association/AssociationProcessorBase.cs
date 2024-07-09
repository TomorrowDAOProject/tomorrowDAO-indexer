using AElf.CSharp.Core;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.NetworkDao.Association;

public abstract class AssociationProcessorBase<TEvent> : NetworkDaoProposalBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly ContractInfoOptions ContractInfoOptions;

    protected AssociationProcessorBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<NetworkDaoProposalIndex, LogEventInfo> proposalRepository) : base(logger,
        objectMapper,
        proposalRepository)
    {
        ContractInfoOptions = contractInfoOptions.Value;
    }

    public override string GetContractAddress(string chainId)
    {
        return ContractInfoOptions.ContractInfos[chainId].AssociationContractAddress;
    }
}