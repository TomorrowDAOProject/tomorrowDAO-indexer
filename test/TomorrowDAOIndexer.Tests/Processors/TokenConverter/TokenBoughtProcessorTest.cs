using AeFinder.Sdk.Processor;
using Shouldly;
using TomorrowDAOIndexer.Entities;
using Xunit;

namespace TomorrowDAOIndexer.Processors.TokenConverter;

public class TokenBoughtProcessorTest : TomorrowDAOIndexerTestBase
{
    [Fact]
    public async Task HandleEventAsync_Test()
    {
        var id = IdGenerateHelper.GetId(ChainId, TransactionId);
        await MockEventProcess(TokenBought(), TokenBoughtProcessor);
        var resourceTokenIndex = await GetIndexById<ResourceTokenIndex>(id);
        resourceTokenIndex.ShouldNotBeNull();
        resourceTokenIndex.ChainId.ShouldBe(ChainId);
         resourceTokenIndex.Id.ShouldBe(id);
         resourceTokenIndex.TransactionId.ShouldBe(TransactionId);
         resourceTokenIndex.Method.ShouldBe(TomorrowDAOConst.TokenConverterContractAddressBuyMethod);
         resourceTokenIndex.Symbol.ShouldBe("WRITE");
         resourceTokenIndex.ResourceAmount.ShouldBe(2);
         resourceTokenIndex.BaseAmount.ShouldBe(1);
         resourceTokenIndex.FeeAmount.ShouldBe(3);
         resourceTokenIndex.BlockHeight.ShouldBe(BlockHeight);
         resourceTokenIndex.TransactionStatus.ShouldBe(TransactionStatus.Mined.ToString());
    }
}