using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Organization;

public class MemberChangedProcessor : OrganizationProcessorBase<MemberChanged>
{
    public MemberChangedProcessor(ILogger<AElfLogEventProcessorBase<MemberChanged, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IOrganizationProvider organizationProvider) :
        base(logger, objectMapper, contractInfoOptions, organizationProvider)
    {
    }

    protected override async Task HandleEventAsync(MemberChanged eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var organizationAddress = eventValue.OrganizationAddress.ToBase58();
        Logger.LogInformation(
            "[MemberChanged] start organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
        var organizationIndex = await OrganizationProvider.GetIndexAsync(context.ChainId, organizationAddress);
        if (organizationIndex == null)
        {
            Logger.LogInformation("[MemberChanged] organizationIndex with id {id} chainId {chainId} has not existed.",
                organizationAddress, chainId);
            return;
        }

        organizationIndex.ChangeMember(eventValue.OldMember?.ToBase58(), eventValue.NewMember?.ToBase58());
        await OrganizationProvider.SaveIndexAsync(organizationIndex, context);
        Logger.LogInformation("[MemberChanged] end organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
    }
}