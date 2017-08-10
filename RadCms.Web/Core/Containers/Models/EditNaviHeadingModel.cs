using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Core.Containers.Models
{
    public class EditNaviHeadingModel
    {
        [Key]
        public int Id { get; set; }

         [Display(Name = "Heading Order")]
        public int HeadingOrder { get; set; }

        [MaxLength(255)]
        [Display(Name = "Web Address")]
        public string Url { get; set; }

        [Required, MaxLength(255), MinLength(1)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Navigation Node")]
        public int? NaviNodeId { get; set; }
    }
}