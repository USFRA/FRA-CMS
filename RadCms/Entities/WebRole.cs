using RadCms.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Entities
{
    public class WebRole : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string RoleName { get; set; }

        [MaxLength(512)]
        public virtual string Description { get; set; }

        public virtual ICollection<WebUser> Users { get; set; }
    }
}