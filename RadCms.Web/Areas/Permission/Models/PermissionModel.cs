using System.Collections.Generic;
using RadCms.Entities;

namespace RadCms.Web.Areas.Permission.Models
{
    public class PermissionModel
    {
        private static readonly Dictionary<PermissionType, string> modes = new Dictionary<PermissionType, string>()
        {
            {PermissionType.Default, "Read"},
            {PermissionType.Webpart, "Webpart"},
            {PermissionType.Edit, "Edit"},
            {PermissionType.Publish, "Publish"},
            {PermissionType.Create, "Create"},
            {PermissionType.Admin, "Admin"}
        };
        public string NodeName { set; get; }
        public PermissionType AccessMode { set; get; }
        public string User { set; get; }
        public string Permission { get {
            return modes[AccessMode];
        } }
    }
}