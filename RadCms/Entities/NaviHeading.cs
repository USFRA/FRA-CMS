using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RadCms.Data;

namespace RadCms.Entities
{
    public class NaviHeading: IEntity
    {
        [Key]
        public int Id { get; set; }

        public int HeadingOrder { get; set; }

        [MaxLength(255)]
        [Display(Name = "Web Address")]
        public string Url { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public virtual NaviNode NaviNode { get; set; }

        public virtual ICollection<NaviLink> NaviLinks { get; set; }
    }
}