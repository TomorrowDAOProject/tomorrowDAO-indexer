using AElf;
using AElf.Types;
using AutoMapper;
using Google.Protobuf;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;

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
    protected static string MapHash(Hash hash)
    {
        return hash.ToHex() ?? string.Empty;
    }
    
    protected static string MapAddress(Address? address)
    {
        return address?.ToBase58() ??  string.Empty;
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
}