using AElf;
using AElf.CSharp.Core.Extension;
using AElf.Types;
using AElfIndexer.Client;
using AElfIndexer.Client.Handlers;
using AElfIndexer.Client.Providers;
using AElfIndexer.Grains;
using AElfIndexer.Grains.State.Client;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Indexer.Orleans.TestBase;
using TomorrowDAO.Indexer.Plugin.Entities;
using TomorrowDAO.Indexer.Plugin.Processors;
using TomorrowDAO.Indexer.Plugin.Tests.Helper;
using File = TomorrowDAO.Contracts.DAO.File;
using FileInfo = TomorrowDAO.Contracts.DAO.FileInfo;
using GovernanceSchemeThreshold = TomorrowDAO.Contracts.DAO.GovernanceSchemeThreshold;
using HighCouncilConfig = TomorrowDAO.Contracts.DAO.HighCouncilConfig;

namespace TomorrowDAO.Indexer.Plugin.Tests;

public abstract class TomorrowDaoIndexerPluginTestBase : TomorrowDaoIndexerOrleansTestBase<TomorrowDAOIndexerPluginTestModule>
{
    private readonly IAElfIndexerClientInfoProvider _indexerClientInfoProvider;
    public IBlockStateSetProvider<LogEventInfo> BlockStateSetLogEventInfoProvider;
    private readonly IBlockStateSetProvider<TransactionInfo> _blockStateSetTransactionInfoProvider;
    private readonly IDAppDataProvider _dAppDataProvider;
    private readonly IDAppDataIndexManagerProvider _dAppDataIndexManagerProvider;
    protected readonly IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo> DaoIndexRepository;
    protected readonly DaoCreatedProcessor DaoCreatedProcessor;
    protected readonly FileInfosRemovedProcessor FileInfosRemovedProcessor;
    
    protected readonly long BlockHeight = 120;
    protected readonly string ChainAelf = "tDVW";
    protected static readonly string Id1 = "123";
    protected static readonly string Id2 = "456";
    protected readonly string DaoId = HashHelper.ComputeFrom(Id1).ToHex();
    protected readonly string DaoName = "DaoName";
    protected readonly string DaoLogoUrl = "DaoLogoUrl";
    protected readonly string DaoDescription = "DaoDescription";
    protected readonly string DaoSocialMedia = "{ \"name\": \"url\" }";
    protected readonly string DaoMetadataAdmin = "2N9DJYUUruS7bFqRyKMvafA75qTWgqpWcB78nNZzpmxHrMv4D";
    protected readonly string Dao = "2N9DJYUUruS7bFqRyKMvafA75qTWgqpWcB78nNZzpmxHrMv4D";
    protected readonly string Elf = "Elf";
    protected readonly string GovernanceSchemeId = HashHelper.ComputeFrom(Id2).ToHex();
    protected readonly string FileHash = "FileHash";
    protected readonly string FileName = "FileName";
    protected readonly string FileUrl = "FileUrl";
    protected readonly string DaoCreator = "2fbCtXNLVD2SC4AD6b8nqAkHtjqxRCfwvciX4MyH6257n8Gf63";
    // protected readonly int MinimalRequiredThreshold = 1;
    // protected readonly int MinimalVoteThreshold = 2;
    // protected readonly int MinimalApproveThreshold = 3;
    // protected readonly int MaximalAbstentionThreshold = 4;
    // protected readonly int MaximalRejectionThreshold = 5;
    protected readonly int MaxHighCouncilCandidateCount = 1;
    protected readonly int MaxHighCouncilMemberCount = 2;
    protected readonly int ElectionPeriod = 3;

    public TomorrowDaoIndexerPluginTestBase()
    {
        _indexerClientInfoProvider = GetRequiredService<IAElfIndexerClientInfoProvider>();
        BlockStateSetLogEventInfoProvider = GetRequiredService<IBlockStateSetProvider<LogEventInfo>>();
        _blockStateSetTransactionInfoProvider = GetRequiredService<IBlockStateSetProvider<TransactionInfo>>();
        _dAppDataProvider = GetRequiredService<IDAppDataProvider>();
        _dAppDataIndexManagerProvider = GetRequiredService<IDAppDataIndexManagerProvider>();
        DaoIndexRepository = GetRequiredService<IAElfIndexerClientEntityRepository<DaoIndex, LogEventInfo>>();
        FileInfosRemovedProcessor = GetRequiredService<FileInfosRemovedProcessor>();
        DaoCreatedProcessor = GetRequiredService<DaoCreatedProcessor>();
    }

    protected async Task<string> InitializeBlockStateSetAsync(BlockStateSet<LogEventInfo> blockStateSet, string chainId)
    {
        var key = GrainIdHelper.GenerateGrainId("BlockStateSets", _indexerClientInfoProvider.GetClientId(), chainId,
            _indexerClientInfoProvider.GetVersion());

        await BlockStateSetLogEventInfoProvider.SetBlockStateSetAsync(key, blockStateSet);
        await BlockStateSetLogEventInfoProvider.SetCurrentBlockStateSetAsync(key, blockStateSet);
        await BlockStateSetLogEventInfoProvider.SetLongestChainBlockStateSetAsync(key, blockStateSet.BlockHash);

        return key;
    }

    protected async Task<string> InitializeBlockStateSetAsync(BlockStateSet<TransactionInfo> blockStateSet,
        string chainId)
    {
        var key = GrainIdHelper.GenerateGrainId("BlockStateSets", _indexerClientInfoProvider.GetClientId(), chainId,
            _indexerClientInfoProvider.GetVersion());

        await _blockStateSetTransactionInfoProvider.SetBlockStateSetAsync(key, blockStateSet);
        await _blockStateSetTransactionInfoProvider.SetCurrentBlockStateSetAsync(key, blockStateSet);
        await _blockStateSetTransactionInfoProvider.SetLongestChainBlockStateSetAsync(key, blockStateSet.BlockHash);

        return key;
    }

    protected async Task BlockStateSetSaveDataAsync<TSubscribeType>(string key)
    {
        await _dAppDataProvider.SaveDataAsync();
        await _dAppDataIndexManagerProvider.SavaDataAsync();
        if (typeof(TSubscribeType) == typeof(TransactionInfo))
            await _blockStateSetTransactionInfoProvider.SaveDataAsync(key);
        else if (typeof(TSubscribeType) == typeof(LogEventInfo))
            await BlockStateSetLogEventInfoProvider.SaveDataAsync(key);
    }

    protected LogEventContext MockLogEventContext(long inputBlockHeight = 100, string chainId = "tDVW")
    {
        const string blockHash = "dac5cd67a2783d0a3d843426c2d45f1178f4d052235a907a0d796ae4659103b1";
        const string previousBlockHash = "e38c4fb1cf6af05878657cb3f7b5fc8a5fcfb2eec19cd76b73abb831973fbf4e";
        const string transactionId = "c1e625d135171c766999274a00a7003abed24cfe59a7215aabf1472ef20a2da2";
        var blockHeight = inputBlockHeight;
        return new LogEventContext
        {
            ChainId = chainId,
            BlockHeight = blockHeight,
            BlockHash = blockHash,
            PreviousBlockHash = previousBlockHash,
            TransactionId = transactionId,
            BlockTime = DateTime.UtcNow,
            ExtraProperties = new Dictionary<string, string>
            {
                { "TransactionFee", "{\"ELF\": 10, \"DECIMAL\": 8}" },
                { "ResourceFee", "{\"ELF\": 10, \"DECIMAL\": 15}" }
            }
        };
    }

    protected LogEventInfo MockLogEventInfo(LogEvent logEvent)
    {
        var logEventInfo = LogEventHelper.ConvertAElfLogEventToLogEventInfo(logEvent);
        var logEventContext = MockLogEventContext(100);
        logEventInfo.BlockHeight = logEventContext.BlockHeight;
        logEventInfo.ChainId = logEventContext.ChainId;
        logEventInfo.BlockHash = logEventContext.BlockHash;
        logEventInfo.TransactionId = logEventContext.TransactionId;
        logEventInfo.BlockTime = DateTime.Now;
        return logEventInfo;
    }

    protected async Task<string> MockBlockState(LogEventContext logEventContext)
    {
        var blockStateSet = new BlockStateSet<LogEventInfo>
        {
            BlockHash = logEventContext.BlockHash,
            BlockHeight = logEventContext.BlockHeight,
            Confirmed = true,
            PreviousBlockHash = logEventContext.PreviousBlockHash
        };
        return await InitializeBlockStateSetAsync(blockStateSet, logEventContext.ChainId);
    }

    protected async Task<string> MockBlockStateForTransactionInfo(LogEventContext logEventContext)
    {
        var blockStateSet = new BlockStateSet<TransactionInfo>
        {
            BlockHash = logEventContext.BlockHash,
            BlockHeight = logEventContext.BlockHeight,
            Confirmed = true,
            PreviousBlockHash = logEventContext.PreviousBlockHash
        };
        return await InitializeBlockStateSetAsync(blockStateSet, logEventContext.ChainId);
    }
    
    protected async Task MockEventProcess(LogEvent logEvent, IAElfLogEventProcessor processor)
    {
        var logEventContext = MockLogEventContext(BlockHeight, ChainAelf);
        
        // step1: create blockStateSet
        var blockStateSetKey = await MockBlockState(logEventContext);
        
        // step2: create logEventInfo
        var logEventInfo = MockLogEventInfo(logEvent);
        
        // step3 call the logic
        await processor.HandleEventAsync(logEventInfo, logEventContext);
        
        // step4 save data after logic
        await BlockStateSetSaveDataAsync<LogEventInfo>(blockStateSetKey);
    }

    protected LogEvent MaxInfoDaoCreated()
    {
        return new DAOCreated
        {
            Metadata = new Metadata
            {
                Name = DaoName,
                LogoUrl = DaoLogoUrl,
                Description = DaoDescription,
                SocialMedia = { ["name"] = "url" }
            },
            MetadataAdmin = Address.FromBase58(DaoMetadataAdmin),
            GovernanceToken = Elf,
            GovernanceSchemeId = HashHelper.ComputeFrom(Id2),
            GovernanceSchemeThreshold = new GovernanceSchemeThreshold
            {
                MinimalRequiredThreshold = 1,
                MinimalVoteThreshold = 2,
                MinimalApproveThreshold = 3,
                MaximalAbstentionThreshold = 4,
                MaximalRejectionThreshold = 5
            },
            IsHighCouncilEnabled = true,
            HighCouncilConfig = new HighCouncilConfig
            {
                MaxHighCouncilCandidateCount = 1,
                MaxHighCouncilMemberCount = 2,
                ElectionPeriod = 3,
                IsRequireHighCouncilForExecution = true
            },
            FileInfoList = GetFileInfoList(),
            IsTreasuryContractNeeded = true,
            IsVoteContractNeeded = true,
            DaoId = HashHelper.ComputeFrom(Id1),
            Creator = Address.FromBase58(DaoCreator)
        }.ToLogEvent();
    }

    protected LogEvent MinInfoDaoCreated()
    {
        return new DAOCreated
        {
            DaoId = HashHelper.ComputeFrom(Id1)
        }.ToLogEvent();
    }

    protected LogEvent FileInfosRemoved()
    {
        return new FileInfosRemoved
        {
            DaoId = HashHelper.ComputeFrom(Id1),
            RemovedFiles = GetFileInfoList()
        }.ToLogEvent();
    }

    protected FileInfoList GetFileInfoList()
    {
        return new FileInfoList
        {
            FileInfos =
            {
                new FileInfo
                {
                    File = new File
                    {
                        Hash = FileHash,
                        Name = FileName,
                        Url = FileUrl
                    }
                }
            }
        };
    }
}