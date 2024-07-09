using AElf.CSharp.Core;
using AElf.Standards.ACS3;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Enums;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.NetworkDao;

public abstract class NetworkDaoProposalBase<TEvent> : AElfLogEventProcessorBase<TEvent, LogEventInfo>
    where TEvent : IEvent<TEvent>, new()
{
    protected readonly ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> Logger;
    protected readonly IObjectMapper ObjectMapper;
    protected readonly IAElfIndexerClientEntityRepository<NetworkDaoProposalIndex, LogEventInfo> ProposalRepository;

    protected NetworkDaoProposalBase(ILogger<AElfLogEventProcessorBase<TEvent, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IAElfIndexerClientEntityRepository<NetworkDaoProposalIndex, LogEventInfo> proposalRepository) : base(logger)
    {
        Logger = logger;
        ObjectMapper = objectMapper;
        ProposalRepository = proposalRepository;
    }

    protected async Task SaveIndexAsync(NetworkDaoProposalIndex index, LogEventContext context)
    {
        ObjectMapper.Map(context, index);
        await ProposalRepository.AddOrUpdateAsync(index);
    }
    
    protected async Task SaveProposalIndex(ProposalCreated eventValue, LogEventContext context, NetworkDaoProposalType proposalType)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId?.ToHex();
        Logger.LogInformation("[ACS3.ProposalCreated] start. chainId={ChainId}, proposalId={ProposalId}", chainId, proposalId);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, proposalId);
            var proposalIndex = await ProposalRepository.GetFromBlockStateSetAsync(id, chainId);
            if (proposalIndex != null)
            {
                Logger.LogInformation("[ACS3.ProposalCreated] Network DAO ProposalIndex already existed id {id}", id);
                return;
            }
            proposalIndex = ObjectMapper.Map<ProposalCreated, NetworkDaoProposalIndex>(eventValue);
            proposalIndex.Id = id;
            proposalIndex.ProposalType = proposalType;
            await SaveIndexAsync(proposalIndex, context);
            Logger.LogInformation("[ACS3.ProposalCreated] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.ProposalCreated] Exception. chainId={ChainId}, proposalId={ProposalId}", chainId, proposalId);
            throw;
        }
    }
}