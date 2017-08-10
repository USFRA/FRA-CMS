using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using RadCms.Data;

namespace RadCms.Entities
{
    public enum RoleType : int
    {
        Normal = 0,
        Super = 1,
        Admin = 2
    }

    public class CmsUser: IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "ADDOT Login")]
        [MaxLength(50), MinLength(1)]
        public string AdName { get; set; }

        [Display(Name = "User Name")]
        [MaxLength(50), MinLength(1)]
        public string UserName { get; set; }

        /**
         * 0 normal
         * 1 super user
         * 2 admin user
         **/
        [Display(Name = "Role")]
        public RoleType RoleId { get; set; }
    }
}
