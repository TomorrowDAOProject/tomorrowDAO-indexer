using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Grains.State.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TomorrowDAO.Contracts.Governance;
using TomorrowDAO.Indexer.Plugin.Entities;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAO.Indexer.Plugin.Processors.Organization;

public class MemberAddedProcessor : OrganizationProcessorBase<MemberAdded>
{
    public MemberAddedProcessor(ILogger<AElfLogEventProcessorBase<MemberAdded, LogEventInfo>> logger,
        IObjectMapper objectMapper,
        IOptionsSnapshot<ContractInfoOptions> contractInfoOptions,
        IAElfIndexerClientEntityRepository<OrganizationIndex, LogEventInfo> organizationRepository) :
        base(logger, objectMapper, contractInfoOptions, organizationRepository)
    {
    }

    protected override async Task HandleEventAsync(MemberAdded eventValue, LogEventContext context)
    {
        var chainId = context.ChainId;
        var organizationAddress = eventValue.OrganizationAddress.ToBase58();
        Logger.LogInformation(
            "[MemberAdded] start organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
        var organizationIndex =
            await OrganizationRepository.GetFromBlockStateSetAsync(organizationAddress, context.ChainId);
        if (organizationIndex == null)
        {
            Logger.LogInformation("[MemberAdded] organizationIndex with id {id} chainId {chainId} has not existed.",
                organizationAddress, chainId);
            return;
        }

        organizationIndex.AddMembers(eventValue.MemberList?.Value.Select(m => m.ToBase58()).ToHashSet());
        await SaveIndexAsync(organizationIndex, context);
        Logger.LogInformation("[MemberAdded] end organizationAddress:{organizationAddress} chainId:{chainId} ",
            organizationAddress, chainId);
    }
}