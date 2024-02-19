using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors.Provider;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Organization;

public class OrganizationCreatedProcessor : OrganizationProcessorBase<OrganizationCreated>
{
    public OrganizationCreatedProcessor(ILogger<AElfLogEventProcessorBase<OrganizationCreated, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IOrganizationProvider organizationProvider) :
        base(logger, objectMapper, contractInfoOptions, organizationProvider)
    {
    }

    protected override async Task HandleEventAsync(OrganizationCreated eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        //use organizationAddress as id
        var organizationAddress = eventValue.OrganizationAddress.ToBase58();
        Logger.LogInformation(
            "[OrganizationCreated] start organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
        var organizationIndex = await OrganizationProvider.GetIndexAsync(context.ChainId, organizationAddress);
        if (organizationIndex != null)
        {
            Logger.LogInformation("[OrganizationCreated] organizationIndex with id {id} chainId {chainId} has existed.",
                organizationAddress, chainId);
            return;
        }
        organizationIndex = ObjectMapper.Map<OrganizationCreated, OrganizationIndex>(eventValue);
        organizationIndex.Id = organizationAddress;
        organizationIndex.CreateTime = context.BlockTime;
        await OrganizationProvider.SaveIndexAsync(organizationIndex, context);
        Logger.LogInformation("[OrganizationCreated] end organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
    }
}