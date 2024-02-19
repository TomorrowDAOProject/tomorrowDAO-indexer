using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Organization;

public class MemberRemovedProcessor : OrganizationProcessorBase<MemberRemoved>
{
    public MemberRemovedProcessor(ILogger<AElfLogEventProcessorBase<MemberRemoved, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions, 
        IOrganizationProvider organizationProvider) :
        base(logger, objectMapper, contractInfoOptions, organizationProvider)
    {
    }

    protected override async Task HandleEventAsync(MemberRemoved eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var organizationAddress = eventValue.OrganizationAddress.ToBase58();
        Logger.LogInformation(
            "[MemberRemoved] start organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
        var organizationIndex = await OrganizationProvider.GetIndexAsync(context.ChainId, organizationAddress);
        if (organizationIndex == null)
        {
            Logger.LogInformation("[MemberRemoved] organizationIndex with id {id} chainId {chainId} has not existed.",
                organizationAddress, chainId);
            return;
        }

        organizationIndex.RemoveMembers(eventValue.MemberList?.Value.Select(m => m.ToBase58()).ToHashSet());
        await OrganizationProvider.SaveIndexAsync(organizationIndex, context);
        Logger.LogInformation("[MemberRemoved] end organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
    }
}