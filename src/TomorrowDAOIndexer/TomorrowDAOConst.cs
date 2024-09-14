namespace TomorrowDAOIndexer;

public class TomorrowDAOConst
{
    public const string Asc = "asc";
    public const string Ascend = "ascend";

    public const string VotesAmount = "votesamount";
    public const string StakeAmount = "stakeamount";

    public const string VoteTime = "votetime";
    public const string Amount = "amount";

    public const int MinActiveTimePeriod = 7; // days
    public const int MaxActiveTimePeriod = 15; // days
    public const int MinPendingTimePeriod = 5; // days
    public const int MaxPendingTimePeriod = 7; // days
    public const int MinExecuteTimePeriod = 3; // days
    public const int MaxExecuteTimePeriod = 5; // days
    public const int MinVetoActiveTimePeriod = 3; // days
    public const int MaxVetoActiveTimePeriod = 5; // days
    public const int MinVetoExecuteTimePeriod = 1; // days
    public const int MaxVetoExecuteTimePeriod = 3; // days

    public const string MainChainId = "AELF";
    public const string TestNetSideChainId = "tDVW";
    public const string MainNetSideChainId = "tDVV";

    //Main Chain
    public const string ParliamentContractAddress = "2JT8xzjR5zJ8xnBvdgBZdSjfbokFSbF5hDdpUCbXeWaJfPDmsK";
    public const string AssociationContractAddress = "XyRN9VNabpBiVUFeX2t7ZUR2b3tWV7U31exufJ2AUepVb5t56";
    public const string ReferendumContractAddress = "NxSBGHE3zs85tpnX1Ns4awQUtFL8Dnr6Hux4C4E18WZsW4zzJ";
    public const string TokenConverterContractAddress = "SietKh9cArYub9ox6E4rU94LrzPad6TB72rCwe3X1jQ5m1C34";
    public const string TokenContractAddress = "JRmBduh4nXWi1aXgdUsj5gJrzeZb2LxmrAbf7W99faZSvoAaE";
    public const string NetworkDaoTreasuryContractAddress = "KNdM6U6PyPsgyena8rPHTbCoMrkrALhxAy1b8Qx2cgi4169xr";

    //TestNet Side Chian
    public const string DAOContractAddressTestNetSideChain = "RRF7deQbmicUh6CZ1R2y7U9M8n2eHPyCgXVHwiSkmNETLbL4D";
    public const string GovernanceContractAddressTestNetSideChain = "2sJ8MDufVDR3V8fDhBPUKMdP84CUf1oJroi9p8Er1yRvMp3fq7";
    public const string VoteContractAddressTestNetSideChain = "2LpoLYNPAnjiBUozyYwcfaeekxRFehjt6hDR78VKgk47UwkvAv";
    public const string ElectionContractAddressTestNetSideChain = "2akycoQt8nYSQbarup4tTJYxQ4SzdKbwBfBCcCF9yqWYtMXt9j";
    public const string TreasuryContractAddressTestNetSideChain = "3FdTVXDuBMVAsXJ598aTm3GifQQey5ahFsonjhanTLs4qnukT";
    public const string TokenContractAddressTestNetSideChain = "ASh2Wt7nSEmYqnGxPPzp4pnVDU4uhj1XW9Se5VeZcX2UDdyjx";
    public const string ParliamentContractAddressTestNetSideChain = "vcv1qewcsFN2tVWqLuu7DJ5wVFA8YEx5FFgCQBb1jMCbAQHxV";
    public const string AssociationContractAddressTestNetSideChain = "MbWXHaAY5sGpngiep6RS2euSzMZ2vHoXgmrfjEn3D1kCc1wbJ";
    public const string ReferendumContractAddressTestNetSideChain = "2cVQrFiXNaedBYmUrovmUV2jcF9Hf6AXbh12gWsD4P49NaX99y";

    //MainNet Side Chain
    public const string DAOContractAddressMainNetSideChain = "2izSidAeMiZ6tmD7FKmnoWbygjFSmH5nko3cGJ9EtbfC44BycC";
    public const string GovernanceContractAddressMainNetSideChain = "2tCM3oV6dTCmwFxSiFGPEVhGngdMwBV741wi156vj8kmqfp6da";
    public const string VoteContractAddressMainNetSideChain = "2A8h4hLynLt86RxqvpNY43x6Js8CYhgyuAzj7sDGQ2ecP77Zgp";
    public const string ElectionContractAddressMainNetSideChain = "QWMSafkNs3oydr7EqktZ7kR2cE8j9c8qAbKymds5FLQZXcPiD";
    public const string TreasuryContractAddressMainNetSideChain = "TUeLJxSYY37kGbK8jf7NcLZ43g8k2DgQufgvzYjnsUzQsoCas";
    public const string TokenContractAddressMainNetSideChain = "7RzVGiuVWkvL4VfVHdZfQF2Tri3sgLe9U991bohHFfSRZXuGX";
    public const string ParliamentContractAddressMainNetSideChain = "4SGo3CUj3PPh3hC5oXV83WodyUaPHuz4trLoSTGFnxe84nqNr";
    public const string AssociationContractAddressMainNetSideChain = "mWU7iE7HEfeZPDRYqdJAFqr2wiGfkiVrFCBuu6x1oL7Zca4KD";
    public const string ReferendumContractAddressMainNetSideChain = "V8NtmXA5TsuZKPK1bJMNGK6Gqomt1abvXeGWEpyMpC77s1toc";

    public const string DateFormat = "yyyy-MM-dd HH:mm:ss";
    
    // public static readonly Dictionary<string, Dictionary<string, List<string>>> TransactionAddressMethodMap = new()
    // {
    //     {
    //         MainChainId, new Dictionary<string, List<string>>
    //         {
    //             { TokenConverterContractAddress, [
    //                 TokenConverterContractAddressBuyMethod, 
    //                 TokenConverterContractAddressSellMethod,
    //                 TokenConverterContractAddressCaMethod
    //             ] }
    //         }
    //     }
    // };

    public static readonly Dictionary<string, string> MethodEventMap = new()
    {
        { TokenConverterContractAddressBuyMethod, "TokenBought" },
        { TokenConverterContractAddressSellMethod, "TokenSold" }
    };
    
    public const string TokenConverterContractAddressBuyMethod = "Buy";
    public const string TokenConverterContractAddressSellMethod = "Sell";
    public const string TokenConverterContractAddressCaMethod = "ManagerForwardCall";
    public const string NetworkDaoId = "e9e131724d50de8fce13629043dc8a58a6692be57417985972418159cd883d72";
    
    // Votigram
    public const string VotigramCollectionSymbolTestNet = "TOMORROWPASSTEST-"; 
    public const string VotigramCollectionSymbolMainNet = "TOMORROWPASS-"; 
}