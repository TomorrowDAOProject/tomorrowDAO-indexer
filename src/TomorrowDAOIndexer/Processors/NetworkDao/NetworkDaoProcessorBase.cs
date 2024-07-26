using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using AElf.Standards.ACS3;
using TomorrowDAOIndexer.Entities;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer.Processors.NetworkDao;

public abstract class NetworkDaoProposalBase<TEvent> : ProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected async Task SaveProposalIndex(ProposalCreated eventValue, LogEventContext context, NetworkDaoProposalType proposalType)
    {
        var chainId = context.ChainId;
        var proposalId = eventValue.ProposalId?.ToHex();
        Logger.LogInformation("[ACS3.ProposalCreated] start. chainId={ChainId}, proposalId={ProposalId}", chainId, proposalId);
        try
        {
            var id = IdGenerateHelper.GetId(chainId, proposalId);
            var proposalIndex = await GetEntityAsync<NetworkDaoProposalIndex>(id);
            if (proposalIndex != null)
            {
                Logger.LogInformation("[ACS3.ProposalCreated] Network DAO ProposalIndex already existed id {id}", id);
                return;
            }
            proposalIndex = ObjectMapper.Map<ProposalCreated, NetworkDaoProposalIndex>(eventValue);
            proposalIndex.Id = id;
            proposalIndex.ProposalType = proposalType;
            await SaveEntityAsync(proposalIndex, context);
            Logger.LogInformation("[ACS3.ProposalCreated] Finished. id {id}", id);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[ACS3.ProposalCreated] Exception. chainId={ChainId}, proposalId={ProposalId}", chainId, proposalId);
            throw;
        }
    }
}