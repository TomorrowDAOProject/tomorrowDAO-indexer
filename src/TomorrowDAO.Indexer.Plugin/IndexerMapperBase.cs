using AElf;
using AutoMapper;
using Google.Protobuf;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.Vote;

namespace TomorrowDAO.Indexer.Plugin;

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
        return hash?.ToHex();
    }
    
    protected static string MapAddress(AElf.Types.Address address)
    {
        return address?.ToBase58();
    }
    
    protected static DateTime? MapDateTime(Google.Protobuf.WellKnownTypes.Timestamp timestamp)
    {
        return timestamp?.ToDateTime();
    }

    protected static string MapByteString([CanBeNull] ByteString byteString)
    {
        return byteString?.ToByteArray().ToHex();
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

    // protected static HashSet<string> MapOrganizationMemberSet(OrganizationCreated source)
    // {
    //     return source.OrganizationMemberList?.OrganizationMembers
    //         ?.Select(MapAddress)
    //         .ToHashSet() ?? new HashSet<string>();
    // }
}