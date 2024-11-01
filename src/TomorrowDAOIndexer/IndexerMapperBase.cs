using AeFinder.Sdk.Processor;
using AElf;
using AElf.Types;
using AutoMapper;
using Google.Protobuf;
using Google.Protobuf.Collections;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;
using TomorrowDAOIndexer.Enums;

namespace TomorrowDAOIndexer;

public class IndexerMapperBase : Profile
{
    public static T MapFromJsonString<T>(string jsonString)
    {
        return JsonConvert.DeserializeObject<T>(jsonString);
    }

    public static string MapToJsonString<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    protected static string MapHash(AElf.Types.Hash hash)
    {
        return hash?.ToHex() ?? string.Empty;
    }

    protected static ReceiptTypeEnum MapReceiptType(string type)
    {
        if (type.IsNullOrWhiteSpace())
        {
            return default;
        }

        if (type == ReceiptTypeEnum.Approve.ToString())
        {
            return ReceiptTypeEnum.Approve;
        }
        else if (type == ReceiptTypeEnum.Reject.ToString())
        {
            return ReceiptTypeEnum.Reject;
        }
        else if (type == ReceiptTypeEnum.Abstain.ToString())
        {
            return ReceiptTypeEnum.Abstain;
        }

        return default;
    }

    protected static string MapAddress(AElf.Types.Address address)
    {
        if (address != null && address.Value.Any())
        {
            return address.ToBase58();
        }

        return string.Empty;
    }

    protected static string MapTransactionStatus(TransactionStatus? status)
    {
        if (status == null)
        {
            return string.Empty;
        }

        return status.ToString();
    }

    protected static DateTime? MapDateTime(Google.Protobuf.WellKnownTypes.Timestamp timestamp)
    {
        return timestamp?.ToDateTime();
    }

    protected static string MapByteString([CanBeNull] ByteString byteString)
    {
        return byteString?.ToByteArray().ToHex();
    }

    protected static string MapByteStringToBase64([CanBeNull] ByteString byteString)
    {
        return byteString?.ToBase64();
    }

    protected static List<string> MapVotingItemIdList(VotingItemIdList votingItemIdList)
    {
        var list = new List<string>();
        if (votingItemIdList == null)
        {
            return list;
        }

        list.AddRange(votingItemIdList.Value.Select(votingItemId => votingItemId?.ToHex()));
        return list;
    }

    protected static List<string> MapAddressList(RepeatedField<Address>? addresses)
    {
        var list = new List<string>();
        if (addresses == null)
        {
            return list;
        }

        list.AddRange(addresses.Select(address => address?.ToBase58() ?? string.Empty));
        return list;
    }
}