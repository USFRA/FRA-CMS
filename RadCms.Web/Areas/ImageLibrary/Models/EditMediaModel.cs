using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace RadCms.Web.Areas.ImageLibrary.Models
{
    public class EditMediaModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Navigation Node")]
        public int? NaviNodeId { get; set; }
    }
}