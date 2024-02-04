namespace TomorrowDAO.Indexer.Plugin.Entities;

public class PermissionInfo
{
    public string Where { get; set; }
    public string What { get; set; }
    public PermissionType PermissionType { get; set; }
    public string Who { get; set; }
}