using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Core.Containers.Models
{
    public class EditNaviLinkModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Link Order")]
        public int LinkOrder { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Web Address")]
        public string Url { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Navigation Heading")]
        public int? NaviHeadingId { get; set; }
    }
}