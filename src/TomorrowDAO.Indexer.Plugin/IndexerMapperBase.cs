using AutoMapper;
using Newtonsoft.Json;
using TomorrowDAO.Contracts.DAO;
using TomorrowDAO.Contracts.Governance;
using PermissionInfoIndexer = TomorrowDAO.Indexer.Plugin.Entities.PermissionInfo;
using PermissionTypeIndexer = TomorrowDAO.Indexer.Plugin.Enums.PermissionType;

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

    protected static HashSet<string> MapOrganizationMemberSet(OrganizationCreated source)
    {
        return source.OrganizationMemberList?.OrganizationMembers
            ?.Select(MapAddress)
            .ToHashSet() ?? new HashSet<string>();
    }
    
    protected static string MapPermissionInfoList(PermissionsSet source)
    {
        return JsonConvert.SerializeObject(source.PermissionInfoList?.PermissionInfos?
            .Select(x => new PermissionInfoIndexer
            {
                Where = MapAddress(x.Where), Who = MapAddress(x.Who), What = x.What,
                PermissionType = (PermissionTypeIndexer)Enum.Parse(typeof(PermissionType), x.PermissionType.ToString(), true)
            }).ToList() ?? new List<PermissionInfoIndexer>());
    }
}