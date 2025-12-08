// using AeFinder.Sdk.Logging;
// using AeFinder.Sdk.Processor;
// using AElf.Contracts.TokenConverter;
// using TomorrowDAOIndexer.Entities;
//
// namespace TomorrowDAOIndexer.Processors;
//
// public class TransactionProcessor : TransactionProcessorBase
// {
//     public override async Task ProcessAsync(Transaction transaction, TransactionContext context)
//     {
//         await ProcessResourceTokenAsync(transaction, context);
//     }
//
//     private async Task ProcessResourceTokenAsync(Transaction transaction, TransactionContext context)
//     {
//         if (!IsContractAddressMethod(transaction, context, TomorrowDAOConst.TransactionAddressMethodMap))
//         {
//             return;
//         }
//         
//         try
//         {
//             Logger.LogInformation("[ProcessResourceToken] START: chainId {treasuryAddress} method {method}", 
//                 context.ChainId, transaction.MethodName);
//             var eventName = TomorrowDAOConst.MethodEventMap.GetValueOrDefault(transaction.MethodName, string.Empty);
//             var isBuy = transaction.MethodName == TomorrowDAOConst.TokenConverterContractAddressBuyMethod;
//             var logEvent = transaction.LogEvents.SingleOrDefault(x => x.EventName == eventName);
//             if (logEvent == null)
//             {
//                 Logger.LogInformation("[ProcessResourceToken] no needed logEvent: chainId {treasuryAddress} method {method}", 
//                     context.ChainId, transaction.MethodName);
//                 return;
//             }
//
//             string symbol;
//             long resourceAmount, baseAmount, feeAmount;
//             if (isBuy)
//             {
//                 var deserializeLogEvent = LogEventDeserializationHelper.DeserializeLogEvent<TokenBought>(logEvent);
//                 symbol = deserializeLogEvent.Symbol;
//                 resourceAmount = deserializeLogEvent.BoughtAmount;
//                 baseAmount = deserializeLogEvent.BaseAmount;
//                 feeAmount = deserializeLogEvent.FeeAmount;
//             }
//             else
//             {
//                 var deserializeLogEvent = LogEventDeserializationHelper.DeserializeLogEvent<TokenSold>(logEvent);
//                 symbol = deserializeLogEvent.Symbol;
//                 resourceAmount = deserializeLogEvent.SoldAmount;
//                 baseAmount = deserializeLogEvent.BaseAmount;
//                 feeAmount = deserializeLogEvent.FeeAmount;
//             }
//
//             await SaveEntityAsync(new ResourceTokenIndex
//             {
//                 Id = IdGenerateHelper.GetId(context.ChainId, transaction.TransactionId),
//                 TransactionId = transaction.TransactionId,
//                 Address = transaction.From,
//                 Method = transaction.MethodName,
//                 Symbol = symbol,
//                 ResourceAmount = resourceAmount,
//                 BaseAmount = baseAmount,
//                 FeeAmount = feeAmount,
//                 ChainId = context.ChainId,
//                 BlockHeight = context.Block.BlockHeight,
//                 TransactionStatus = transaction.Status.ToString(),
//                 OperateTime = context.Block.BlockTime
//             });
//             Logger.LogInformation("[ProcessResourceToken] FINISH: chainId {treasuryAddress} method {method}", 
//                 context.ChainId, transaction.MethodName);
//         }
//         catch(Exception e)
//         {
//             Logger.LogError(e, "[ProcessResourceToken] Exception: chainId {treasuryAddress} method {method}", context.ChainId, transaction.MethodName);
//             throw;
//         }
//     }
//
//     private static bool IsContractAddressMethod(Transaction transaction, ContextBase context, IReadOnlyDictionary<string, Dictionary<string, List<string>>> map)
//     {
//         if (transaction.MethodName.IsNullOrEmpty() || context.ChainId.IsNullOrEmpty() || transaction.MethodName.IsNullOrEmpty())
//         {
//             return false;
//         }
//
//         return map.TryGetValue(context.ChainId, out var addressMethodMap) 
//                && addressMethodMap.TryGetValue(transaction.To, out var methodList) 
//                && methodList.Contains(transaction.MethodName);
//     }
// }