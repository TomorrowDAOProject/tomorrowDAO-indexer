using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TomorrowDAO.Indexer.Plugin.Entities;

public class PermissionInfo
{
    [Keyword] public string Where { get; set; }
    [Keyword] public string What { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public PermissionType PermissionType { get; set; }
    [Keyword] public string Who { get; set; }
}