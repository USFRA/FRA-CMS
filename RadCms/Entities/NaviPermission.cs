using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{

    public class NaviPermission: IEntity
    {
        [Key]
        public int Id { get; set; }

        public virtual CmsUser User { get; set; }

        public virtual NaviNode Section { get; set; }

        /*
         * 1 - Write
         * 2 - Publish
         */
        public PermissionType AccessMode { get; set; }
    }
}