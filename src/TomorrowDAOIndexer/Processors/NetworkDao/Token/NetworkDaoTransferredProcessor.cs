// using AeFinder.Sdk.Logging;
// using AeFinder.Sdk.Processor;
// using AElf.Contracts.MultiToken;
// using TomorrowDAOIndexer.Entities;
// using TomorrowDAOIndexer.Enums;
//
// namespace TomorrowDAOIndexer.Processors.NetworkDao.Token;
//
// public class NetworkDaoTransferredProcessor : MainChainTokenProcessorBase<Transferred>
// {
//     public override async Task ProcessAsync(Transferred logEvent, LogEventContext context)
//     {
//         var toAddress = logEvent.To?.ToBase58() ?? string.Empty;
//         var fromAddress = logEvent.From?.ToBase58() ?? string.Empty;
//         var chainId = context.ChainId;
//         var symbol = logEvent.Symbol;
//         var isOut = fromAddress == TomorrowDAOConst.NetworkDaoTreasuryContractAddress;
//         var isIn = toAddress == TomorrowDAOConst.NetworkDaoTreasuryContractAddress;
//         if (!isOut && !isIn)
//         {
//             return;
//         }
//         
//         try
//         {
//             Logger.LogInformation("[NetworkDaoTransferred] START: ChainId {ChainId}, toAddress {toAddress} fromAddress {fromAddress}",
//                 chainId, toAddress, fromAddress);
//             var treasuryRecordType = isOut ? TreasuryRecordType.TransferOut : TreasuryRecordType.TransferIn;
//             var deltaAmount = isOut ? -logEvent.Amount : logEvent.Amount;
//             var treasuryRecordIndex = ObjectMapper.Map<Transferred, TreasuryRecordIndex>(logEvent);
//             treasuryRecordIndex.Id = IdGenerateHelper.GetId(chainId, context.Transaction.TransactionId, fromAddress,treasuryRecordType);
//             treasuryRecordIndex.DaoId = TomorrowDAOConst.NetworkDaoId;
//             treasuryRecordIndex.TreasuryAddress = TomorrowDAOConst.TokenContractAddress;
//             treasuryRecordIndex.TreasuryRecordType = treasuryRecordType;
//             await TreasuryFundStatistic(chainId, TomorrowDAOConst.NetworkDaoId, symbol, toAddress, deltaAmount, context);
//             await TreasuryFundSumStatistic(chainId, symbol, logEvent.Amount, context);
//
//             Logger.LogInformation("[NetworkDaoTransferred] FINISH: ChainId {ChainId}, toAddress {toAddress} fromAddress {fromAddress}",
//                 chainId, toAddress, fromAddress);
//         }
//         catch (Exception e)
//         {
//             Logger.LogError(e, "[NetworkDaoTransferred] Exception: ChainId {ChainId}, toAddress {toAddress} fromAddress {fromAddress}",
//                 chainId, toAddress, fromAddress);
//             throw;
//         }
//     }
// }