// using AElf.CSharp.Core;
// using TomorrowDAOIndexer.Processors.Common;
//
// namespace TomorrowDAOIndexer.Processors.NetworkDao.Token;
//
// public abstract class MainChainTokenProcessorBase<TEvent> : StatisticProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
// {
//     public override string GetContractAddress(string chainId)
//     {
//         return chainId switch
//         {
//             TomorrowDAOConst.MainChainId => TomorrowDAOConst.TokenContractAddress,
//             _ => string.Empty
//         };
//     }
// }