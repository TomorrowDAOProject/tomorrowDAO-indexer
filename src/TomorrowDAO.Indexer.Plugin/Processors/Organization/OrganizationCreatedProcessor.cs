using AElfIndexer.Client;
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
        IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> organizationRepository,
        IGovernanceProvider governanceProvider) :
        base(logger, objectMapper, contractInfoOptions, organizationRepository, governanceProvider)
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
        var organizationIndex = await OrganizationRepository.GetFromBlockStateSetAsync(organizationAddress, context.ChainId);
        if (organizationIndex != null)
        {
            Logger.LogInformation("[OrganizationCreated] organizationIndex with id {id} chainId {chainId} has existed.",
                organizationAddress, chainId);
            return;
        }
        
        organizationIndex = ObjectMapper.Map<OrganizationCreated, OrganizationIndex>(eventValue);
        ObjectMapper.Map(context, organizationIndex);
        organizationIndex.Id = organizationAddress;
        organizationIndex.CreateTime = context.BlockTime;
        await OrganizationRepository.AddOrUpdateAsync(organizationIndex);
        Logger.LogInformation("[OrganizationCreated] end organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
    }
}