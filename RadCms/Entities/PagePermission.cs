using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class PagePermission: IEntity
    {
        [Key]
        public int Id { get; set; }

        public virtual CmsUser User { get; set; }
        
        public virtual CmsPage Page { get; set; }

        /*
         * 1 - Webpart
         * 2 - 
         * 3 - 
         * 4 - 
         */
        public PermissionType AccessMode { get; set; }
    }
}