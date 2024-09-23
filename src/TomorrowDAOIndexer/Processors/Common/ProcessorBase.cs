using AeFinder.Sdk.Entities;
using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using Google.Protobuf;
using Portkey.Contracts.CA;
using TomorrowDAOIndexer.Entities.Base;
using Volo.Abp.ObjectMapping;

namespace TomorrowDAOIndexer.Processors.Common;

public abstract class ProcessorBase<TEvent> : LogEventProcessorBase<TEvent> where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    private ISet<string> CAAddressSet = new HashSet<string>()
    {
        TomorrowDAOConst.PortKeyContractAddress1,
        TomorrowDAOConst.PortKeyContractAddress2,
        TomorrowDAOConst.PortKeyContractAddress1TestNetSideChain,
        TomorrowDAOConst.PortKeyContractAddress2TestNetSideChain,
        TomorrowDAOConst.PortKeyContractAddress1MainNetSideChain,
        TomorrowDAOConst.PortKeyContractAddress2MainNetSideChain
    };

    protected async Task SaveEntityAsync<TEntity>(TEntity index, LogEventContext context) where TEntity : AeFinderEntity
    {
        ObjectMapper.Map(context, index);

        if (index is ITransactionEntity entity)
        {
            var toAddress = context.Transaction.To;
            var methodName = context.Transaction?.MethodName;
            var isPortKeyContract = CAAddressSet.Contains(toAddress);
            var isAAForwardCall = isPortKeyContract && methodName == TomorrowDAOConst.PortKeyContactManagerForwardCall;
            var caContractAddress = string.Empty;
            var caHash = string.Empty;
            var realTo = string.Empty;
            var realMethodName = string.Empty;
            if (isAAForwardCall)
            {
                var managerForwardCallInput =
                    ManagerForwardCallInput.Parser.ParseFrom(ByteString.FromBase64(context.Transaction?.Params));

                caContractAddress = toAddress;
                caHash = managerForwardCallInput.CaHash.ToHex();
                realTo = managerForwardCallInput.ContractAddress.ToBase58();
                realMethodName = managerForwardCallInput.MethodName;
            }

            entity.TransactionInfo = new TransactionInfo
            {
                ChainId = context.ChainId,
                TransactionId = context.Transaction.TransactionId,
                From = context.Transaction.From,
                To = toAddress,
                MethodName = methodName,
                IsAAForwardCall = isAAForwardCall,
                PortKeyContract = caContractAddress,
                CAHash = caHash,
                RealTo = realTo,
                RealMethodName = realMethodName
            };
        }

        await SaveEntityAsync(index);
    }

    private
        protected async Task DeleteEntityAsyncAndCheck<TEntity>(string id) where TEntity : AeFinderEntity
    {
        var index = await GetEntityAsync<TEntity>(id);
        if (index != null)
        {
            await DeleteEntityAsync<TEntity>(id);
        }
    }
}