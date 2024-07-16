using AeFinder.Sdk.Logging;
using AeFinder.Sdk.Processor;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Treasury;
using TomorrowDAOIndexer.Entities;
using Volo.Abp.DependencyInjection;

namespace TomorrowDAOIndexer.Processors.Treasury;

public class TreasuryCreatedProcessor : TreasuryProcessorBase<TreasuryCreated>
{
    public override async Task ProcessAsync(TreasuryCreated logEvent, LogEventContext context)
    {
        var daoId = logEvent.DaoId.ToHex();
        var chainId = context.ChainId;
        var treasuryAddress = logEvent.TreasuryAccountAddress.ToBase58();
        Logger.LogInformation("[TreasuryCreated] START: Id={Id}, ChainId={ChainId}, Event={Event}",
            daoId, chainId, JsonConvert.SerializeObject(logEvent));
        try
        {
            var daoIndex = await GetEntityAsync<DAOIndex>(daoId);
            if (daoIndex != null)
            {
                Logger.LogInformation("[TreasuryCreated] update treasury address: DAOId={DAOId}, ChainId={ChainId}",
                    daoId, chainId);
                daoIndex.TreasuryAccountAddress = treasuryAddress;
                await SaveEntityAsync(daoIndex, context);
            }

            var id = IdGenerateHelper.GetId(chainId, treasuryAddress);
            var treasuryCreateIndex = await GetEntityAsync<TreasuryCreateIndex>(id);
            if (treasuryCreateIndex != null)
            {
                Logger.LogError("[TreasuryCreated] TreasuryCreateIndex exist: Id={Id}, index={Index}",
                    id, JsonConvert.SerializeObject(treasuryCreateIndex));
            }
            else
            {
                await SaveEntityAsync(new TreasuryCreateIndex
                {
                    Id = id,
                    DaoId = daoId,
                    TreasuryAddress = treasuryAddress
                }, context);
                Logger.LogInformation("[TreasuryCreated] TreasuryCreateIndex Updated: Id={Id}, daoId={DaoId}",
                    id, daoId);
            }

            Logger.LogInformation("[TreasuryCreated] FINISH: Id={Id}, ChainId={ChainId}", daoId, chainId);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "[TreasuryCreated] Exception Id={DAOId}, ChainId={ChainId}", daoId, chainId);
            throw;
        }
    }
}