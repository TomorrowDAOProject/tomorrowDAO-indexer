using AutoMapper;
using TomorrowDAO.Contracts.Governance;

namespace TomorrowDAO.Indexer.Plugin;

public class IndexerMapperBase : Profile
{
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

    // protected static HashSet<string> MapOrganizationMemberSet(OrganizationCreated source)
    // {
    //     return source.OrganizationMemberList?.OrganizationMembers
    //         ?.Select(MapAddress)
    //         .ToHashSet() ?? new HashSet<string>();
    // }
}