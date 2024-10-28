// using AeFinder.Sdk.Processor;
// using Google.Protobuf;
// using Google.Protobuf.Collections;
// using Shouldly;
// using TomorrowDAOIndexer.Entities;
// using Xunit;
//
// namespace TomorrowDAOIndexer.Processors;
//
// public class TransactionProcessorTest : TomorrowDAOIndexerTestBase
// {
//     [Fact]
//     public async Task ProcessAsync_Test()
//     {
//         var id = IdGenerateHelper.GetId(ChainId, "tx1");
//         await TransactionProcessor.ProcessAsync(ResourceTokenTransaction("tx1", string.Empty, string.Empty), GenerateTransactionContext());
//         var resourceTokenIndex = await GetIndexById<ResourceTokenIndex>(id);
//         resourceTokenIndex.ShouldBeNull();
//         
//         // real tx id in test net
//         var indexed = new RepeatedField<ByteString>();
//         var byteString = ByteString.CopyFrom(Convert.FromBase64String("CgVXUklURQ=="));
//         indexed.Add(byteString);
//         var nonIndexed = ByteString.FromBase64("EIDKte4BGLj44gQggIcD");
//         var extraProperties = new Dictionary<string, string>
//         {
//             { "Indexed", indexed.ToString() },
//             { "NonIndexed", nonIndexed.ToBase64() }
//         };
//         id = IdGenerateHelper.GetId(ChainId, TokenSoldTransactionId);
//         await TransactionProcessor.ProcessAsync(ResourceTokenTransaction(TokenSoldTransactionId, 
//             TomorrowDAOConst.TokenConverterContractAddressSellMethod, "TokenSold", extraProperties), GenerateTransactionContext());
//         resourceTokenIndex = await GetIndexById<ResourceTokenIndex>(id);
//         resourceTokenIndex.ShouldNotBeNull();
//         resourceTokenIndex.ChainId.ShouldBe(ChainId);
//         resourceTokenIndex.Id.ShouldBe(id);
//         resourceTokenIndex.TransactionId.ShouldBe(TokenSoldTransactionId);
//         resourceTokenIndex.Address.ShouldBe(User);
//         resourceTokenIndex.Method.ShouldBe(TomorrowDAOConst.TokenConverterContractAddressSellMethod);
//         resourceTokenIndex.Symbol.ShouldBe("WRITE");
//         resourceTokenIndex.ResourceAmount.ShouldBe(500000000);
//         resourceTokenIndex.BaseAmount.ShouldBe(10009656);
//         resourceTokenIndex.FeeAmount.ShouldBe(50048);
//         resourceTokenIndex.BlockHeight.ShouldBe(BlockHeight);
//         resourceTokenIndex.TransactionStatus.ShouldBe(TransactionStatus.Mined.ToString());
//         
//         // real tx id in test net
//         indexed = new RepeatedField<ByteString>();
//         byteString = ByteString.CopyFrom(Convert.FromBase64String("CgVXUklURQ=="));
//         indexed.Add(byteString);
//         nonIndexed = ByteString.FromBase64("EIDC1y8Yj5h6IJpO");
//         extraProperties = new Dictionary<string, string>
//         {
//             { "Indexed", indexed.ToString() },
//             { "NonIndexed", nonIndexed.ToBase64() }
//         };
//         id = IdGenerateHelper.GetId(ChainId, TokenBoughtTransactionId);
//         await TransactionProcessor.ProcessAsync(ResourceTokenTransaction(TokenBoughtTransactionId, 
//             TomorrowDAOConst.TokenConverterContractAddressBuyMethod, "TokenBought", extraProperties), GenerateTransactionContext());
//         resourceTokenIndex = await GetIndexById<ResourceTokenIndex>(id);
//         resourceTokenIndex.ShouldNotBeNull();
//         resourceTokenIndex.ChainId.ShouldBe(ChainId);
//         resourceTokenIndex.Id.ShouldBe(id);
//         resourceTokenIndex.TransactionId.ShouldBe(TokenBoughtTransactionId);
//         resourceTokenIndex.Address.ShouldBe(User);
//         resourceTokenIndex.Method.ShouldBe(TomorrowDAOConst.TokenConverterContractAddressBuyMethod);
//         resourceTokenIndex.Symbol.ShouldBe("WRITE");
//         resourceTokenIndex.ResourceAmount.ShouldBe(100000000);
//         resourceTokenIndex.BaseAmount.ShouldBe(2001935);
//         resourceTokenIndex.FeeAmount.ShouldBe(10010);
//         resourceTokenIndex.BlockHeight.ShouldBe(BlockHeight);
//         resourceTokenIndex.TransactionStatus.ShouldBe(TransactionStatus.Mined.ToString());
//     }
// }