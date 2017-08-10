using System.ComponentModel.DataAnnotations;
using RadCms.Data;

namespace RadCms.Entities
{
    public enum PermissionType : int
    {
        Denied = -1,
        Default = 0,
        Webpart = 1,
        Edit = 2,
        Publish = 3,
        Create = 4,
        Admin = 5
    }

    public class Permission: IEntity
    {
        
        //public const int PERMISSION_DENIED = -1;
        //public const int PERMISSION_DEFAULT = 0;
        //public const int PERMISSION_READ = 1;
        //public const int PERMISSION_WRITE = 2;
        //public const int PERMISSION_PUBLISH = 3;
        //public const int PERMISSION_CREATE = 4;
        //public const int PERMISSION_ADMIN = 5;

        [Key]
        public int Id { get; set; }

        public virtual CmsUser User { get; set; }

        [Required, MaxLength(50), MinLength(1)]
        public string Target { get; set; }

        /*
         * 1 - Write
         * 2 - Publish
         */
        public PermissionType AccessMode { get; set; }
    }
}